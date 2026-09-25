using AccessDoor.Core;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace AccessDoor.Desktop;

internal sealed class MainForm : Form
{
    private static readonly Color Brand = Color.FromArgb(56, 198, 164);

    private readonly SettingsStore _settings;
    // The WMI query behind the device token takes up to a second; run it in the background during
    // startup (it is only needed when the user signs in, not when a saved session is resumed).
    private readonly Task<string> _deviceToken = Task.Run(DeviceToken.Get);

    private readonly TableLayoutPanel _loginPanel;
    private readonly TextBox _urlBox = new() { Text = LoginRequest.DefaultBaseUrl, Dock = DockStyle.Fill };
    private readonly TextBox _usernameBox = new() { Dock = DockStyle.Fill };
    private readonly TextBox _keyBox = new() { UseSystemPasswordChar = true, Dock = DockStyle.Fill };
    private readonly Label _errorLabel = new() { ForeColor = Color.DarkRed, AutoSize = true, Anchor = AnchorStyles.None };
    private readonly Button _loginButton = new() { Text = "Login", AutoSize = true, Anchor = AnchorStyles.None };

    private readonly Panel _browserPanel = new() { Dock = DockStyle.Fill, Visible = false };
    private readonly Button _logoutButton = new()
    {
        Text = "Log Out", Dock = DockStyle.Top, Height = 36,
        BackColor = Color.FromArgb(192, 0, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
    };
    private readonly WebView2 _browser = new() { Dock = DockStyle.Fill };
    private readonly PictureBox _loading = new()
    {
        Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage, BackColor = Color.White, Visible = false,
        Image = LoadImage("loading.gif"),
    };

    private Uri? _pendingLogin;

    public MainForm(SettingsStore settings)
    {
        _settings = settings;

        Text = "AccessDoor";
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        ClientSize = new Size(500, 680);
        MinimumSize = new Size(420, 560);
        BackColor = Brand;
        AcceptButton = _loginButton;

        _loginPanel = BuildLoginPanel();
        _browserPanel.Controls.Add(_browser);
        _browserPanel.Controls.Add(_logoutButton);

        Controls.Add(_loading);
        Controls.Add(_browserPanel);
        Controls.Add(_loginPanel);

        _loginButton.Click += async (_, _) => await LoginAsync();
        _logoutButton.Click += (_, _) => Logout();
        _browser.NavigationCompleted += OnNavigationCompleted;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        try
        {
            var env = await CoreWebView2Environment.CreateAsync(
                userDataFolder: Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AccessDoor", "WebView2"));
            await _browser.EnsureCoreWebView2Async(env);
        }
        catch (WebView2RuntimeNotFoundException)
        {
            MessageBox.Show(this,
                "Microsoft Edge WebView2 Runtime is required. Install it from https://go.microsoft.com/fwlink/p/?LinkId=2124703",
                "AccessDoor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
        }

        if (_settings.LoggedInUrl is { } url)
            Navigate(url);
    }

    private async Task LoginAsync()
    {
        _loginButton.Enabled = false;
        LoginRequest request;
        try
        {
            request = new LoginRequest(_urlBox.Text, _usernameBox.Text, _keyBox.Text, await _deviceToken);
        }
        finally
        {
            _loginButton.Enabled = true;
        }

        if (request.Validate() is { } error)
        {
            _errorLabel.Text = error;
            return;
        }

        _errorLabel.Text = string.Empty;
        _keyBox.Clear();
        Navigate(request.ToUri());
    }

    private void Navigate(Uri url)
    {
        _pendingLogin = url;
        _loginPanel.Visible = false;
        _loading.Visible = true;
        _loading.BringToFront();
        _browser.Source = url; // Also starts WebView2 if initialization has not finished yet.
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        _loading.Visible = false;
        if (_pendingLogin is null)
            return; // In-app navigation after a successful login.

        var result = LoginResult.TryParse(_browser.Source?.AbsoluteUri);
        if (!e.IsSuccess || result is not { IsAllowed: true })
        {
            ShowLogin(result?.Message ?? (e.IsSuccess
                ? "The server did not confirm the login."
                : $"Could not reach the server ({e.WebErrorStatus})."));
            return;
        }

        _settings.LoggedInUrl = _pendingLogin;
        _settings.Save();
        _pendingLogin = null;
        _browserPanel.Visible = true;
        _browserPanel.BringToFront();
    }

    private void Logout()
    {
        _settings.LoggedInUrl = null;
        _settings.Save();
        // Drop the portal's session cookies so the next user starts clean.
        _browser.CoreWebView2?.CookieManager.DeleteAllCookies();
        _browser.CoreWebView2?.Navigate("about:blank");
        ShowLogin(string.Empty);
    }

    private void ShowLogin(string message)
    {
        _pendingLogin = null;
        _browserPanel.Visible = false;
        _loading.Visible = false;
        _errorLabel.Text = message;
        _loginPanel.Visible = true;
        _loginPanel.BringToFront();
        _usernameBox.Focus();
    }

    private TableLayoutPanel BuildLoginPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(60, 40, 60, 40), BackColor = Brand,
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var logo = new PictureBox
        {
            Image = LoadImage("login.png"), SizeMode = PictureBoxSizeMode.Zoom,
            Size = new Size(160, 160), Anchor = AnchorStyles.None, Margin = new Padding(0, 20, 0, 30),
        };
        panel.Controls.Add(logo);
        panel.SetColumnSpan(logo, 2);

        AddRow(panel, "URL", _urlBox);
        AddRow(panel, "Username", _usernameBox);
        AddRow(panel, "Key", _keyBox);

        panel.Controls.Add(_errorLabel);
        panel.SetColumnSpan(_errorLabel, 2);
        panel.Controls.Add(_loginButton);
        panel.SetColumnSpan(_loginButton, 2);
        return panel;
    }

    private static void AddRow(TableLayoutPanel panel, string label, Control input)
    {
        panel.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 10, 10, 10) });
        input.Margin = new Padding(0, 8, 0, 8);
        panel.Controls.Add(input);
    }

    private static Image LoadImage(string name)
    {
        using var stream = typeof(MainForm).Assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Missing embedded resource '{name}'.");
        // Copy into memory: GDI+ needs the stream alive for animated GIFs.
        var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return Image.FromStream(buffer);
    }
}

using AccessDoor.Core;
using AccessDoor.Mobile.Services;

namespace AccessDoor.Mobile.Views;

public partial class LogInPage : ContentPage
{
    private readonly SessionStore _session;
    private readonly Navigator _navigator;
    private bool _signingIn;
    private bool _sessionChecked;

    public LogInPage(SessionStore session, Navigator navigator)
    {
        InitializeComponent();
        _session = session;
        _navigator = navigator;

        UrlEntry.Text = LoginRequest.DefaultBaseUrl;
        UrlEntry.Completed += (_, _) => UsernameEntry.Focus();
        UsernameEntry.Completed += (_, _) => KeyEntry.Focus();
        KeyEntry.Completed += OnSignInClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Resume a saved session instead of showing the form. After logout the session is
        // already cleared, so this falls through to the form.
        if (_sessionChecked)
            return;
        _sessionChecked = true;

        if (await _session.GetLoggedInUrlAsync() is { } url)
            _navigator.ShowPortal(url);
        else
            Form.IsVisible = true;
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        if (_signingIn)
            return; // Ignore double taps / Enter + tap.

        var request = new LoginRequest(UrlEntry.Text ?? "", UsernameEntry.Text ?? "", KeyEntry.Text ?? "", _session.DeviceToken);
        if (request.Validate() is { } error)
        {
            await DisplayAlertAsync("Login", error, "OK");
            return;
        }

        _signingIn = true;
        _navigator.ShowPortal(request.ToUri());
    }
}

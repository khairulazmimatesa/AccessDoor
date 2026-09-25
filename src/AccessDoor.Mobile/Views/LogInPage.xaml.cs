using AccessDoor.Core;
using AccessDoor.Mobile.Services;

namespace AccessDoor.Mobile.Views;

public partial class LogInPage : ContentPage
{
    private readonly SessionStore _session;

    public LogInPage(SessionStore session)
    {
        InitializeComponent();
        _session = session;

        UrlEntry.Text = LoginRequest.DefaultBaseUrl;
        UrlEntry.Completed += (_, _) => UsernameEntry.Focus();
        UsernameEntry.Completed += (_, _) => KeyEntry.Focus();
        KeyEntry.Completed += OnSignInClicked;
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        var request = new LoginRequest(UrlEntry.Text ?? "", UsernameEntry.Text ?? "", KeyEntry.Text ?? "", _session.DeviceToken);

        if (request.Validate() is { } error)
        {
            await DisplayAlertAsync("Login", error, "OK");
            return;
        }

        KeyEntry.Text = string.Empty;
        await Navigation.PushAsync(new WebPage(request.ToUri(), _session));
    }
}

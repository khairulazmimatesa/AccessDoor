using AccessDoor.Core;
using AccessDoor.Mobile.Services;

namespace AccessDoor.Mobile.Views;

public partial class WebPage : ContentPage
{
    private readonly Uri _loginUrl;
    private readonly SessionStore _session;
    private readonly Navigator _navigator;
    private bool _loginConfirmed;

    public WebPage(Uri loginUrl, SessionStore session, Navigator navigator)
    {
        InitializeComponent();
        _loginUrl = loginUrl;
        _session = session;
        _navigator = navigator;
        Browser.Source = loginUrl.AbsoluteUri;
    }

    private async void OnNavigated(object? sender, WebNavigatedEventArgs e)
    {
        LoadingPanel.IsVisible = false;

        switch (e.Result)
        {
            case WebNavigationResult.Success:
                if (_loginConfirmed)
                    break; // In-app navigation after a successful login.

                // The login response must explicitly allow access; a page without a result
                // (wrong URL, captive portal, ...) is not a successful login.
                var result = LoginResult.TryParse(e.Url);
                if (result is { IsAllowed: true })
                    await ShowBrowserAsync();
                else
                    ShowError(result?.Message ?? "The server did not confirm the login.", canRetry: false);
                break;

            case WebNavigationResult.Timeout:
                ShowError("The server did not respond. Please contact your system admin.", canRetry: true);
                break;

            case WebNavigationResult.Failure:
                ShowError("Please check your internet connection or contact your system admin.", canRetry: true);
                break;

            // Cancel: a newer navigation superseded this one; wait for its result.
        }
    }

    private async Task ShowBrowserAsync()
    {
        _loginConfirmed = true;
        Browser.IsVisible = true;
        ErrorPanel.IsVisible = false;
        NavigationPage.SetHasNavigationBar(this, true);
        await _session.SetLoggedInUrlAsync(_loginUrl);
    }

    private void ShowError(string message, bool canRetry)
    {
        Browser.IsVisible = false;
        ErrorMessage.Text = message;
        RetryButton.IsVisible = canRetry;
        ErrorPanel.IsVisible = true;
    }

    private void OnRetryClicked(object? sender, EventArgs e)
    {
        ErrorPanel.IsVisible = false;
        LoadingPanel.IsVisible = true;
        Browser.Source = new UrlWebViewSource { Url = _loginUrl.AbsoluteUri };
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        _session.ClearLoggedInUrl();
        _navigator.ShowLogin();
    }
}

using AccessDoor.Core;
using AccessDoor.Mobile.Services;

namespace AccessDoor.Mobile.Views;

public partial class WebPage : ContentPage
{
    private readonly Uri _loginUrl;
    private readonly SessionStore _session;

    public WebPage(Uri loginUrl, SessionStore session)
    {
        InitializeComponent();
        _loginUrl = loginUrl;
        _session = session;
        Browser.Source = loginUrl.AbsoluteUri;
    }

    private void OnNavigated(object? sender, WebNavigatedEventArgs e)
    {
        LoadingPanel.IsVisible = false;

        switch (e.Result)
        {
            case WebNavigationResult.Success:
                var result = LoginResult.TryParse(e.Url);
                if (result is { IsAllowed: false })
                    ShowError(result.Message ?? "Access denied.", canRetry: false);
                else
                    ShowBrowser();
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

    private void ShowBrowser()
    {
        Browser.IsVisible = true;
        ErrorPanel.IsVisible = false;
        NavigationPage.SetHasNavigationBar(this, true);
        _session.LoggedInUrl = _loginUrl;
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
        _session.LoggedInUrl = null;
        var loginPage = Handler!.MauiContext!.Services.GetRequiredService<LogInPage>();
        Window!.Page = new NavigationPage(loginPage);
    }
}

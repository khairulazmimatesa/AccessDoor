using AccessDoor.Mobile.Views;

namespace AccessDoor.Mobile.Services;

/// <summary>
/// Swaps the window's root page. Login and the portal are never stacked, so the previous page
/// (and its WebView) is released and the Android back button cannot return to a stale screen.
/// </summary>
public sealed class Navigator(IServiceProvider services, SessionStore session)
{
    public void ShowLogin() => SetRoot(services.GetRequiredService<LogInPage>());

    public void ShowPortal(Uri loginUrl) => SetRoot(new WebPage(loginUrl, session, this));

    private static void SetRoot(Page page)
    {
        var window = Application.Current?.Windows is [var first, ..]
            ? first
            : throw new InvalidOperationException("No window to navigate in.");
        window.Page = new NavigationPage(page);
    }
}

using AccessDoor.Mobile.Services;
using AccessDoor.Mobile.Views;

namespace AccessDoor.Mobile;

public partial class App : Application
{
    private readonly SessionStore _session;
    private readonly IServiceProvider _services;

    public App(SessionStore session, IServiceProvider services)
    {
        InitializeComponent();
        _session = session;
        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page root = _session.LoggedInUrl is { } url
            ? new WebPage(url, _session)
            : _services.GetRequiredService<LogInPage>();

        return new Window(new NavigationPage(root));
    }
}

using AccessDoor.Mobile.Views;

namespace AccessDoor.Mobile;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
    }

    // The login page restores a saved session itself (secure storage is async).
    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new NavigationPage(_services.GetRequiredService<LogInPage>()));
}

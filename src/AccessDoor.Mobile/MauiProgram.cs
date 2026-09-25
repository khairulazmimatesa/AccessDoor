using AccessDoor.Mobile.Services;
using AccessDoor.Mobile.Views;

namespace AccessDoor.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<SessionStore>();
        builder.Services.AddSingleton<IPreferences>(Preferences.Default);
        builder.Services.AddTransient<LogInPage>();

        return builder.Build();
    }
}

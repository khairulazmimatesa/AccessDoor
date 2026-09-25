using AccessDoor.Mobile.Services;
using AccessDoor.Mobile.Views;

namespace AccessDoor.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddSingleton(SecureStorage.Default);
        builder.Services.AddSingleton<SessionStore>();
        builder.Services.AddSingleton<Navigator>();
        builder.Services.AddTransient<LogInPage>();

        return builder.Build();
    }
}

using System.Diagnostics.CodeAnalysis;
using Foundation;

namespace AccessDoor.Mobile;

[Register("AppDelegate")]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
    Justification = "AppDelegate is the standard name of the iOS application delegate.")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

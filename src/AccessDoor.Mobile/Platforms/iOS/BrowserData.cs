using Foundation;
using WebKit;

namespace AccessDoor.Mobile.Services;

internal static partial class BrowserData
{
    public static partial Task ClearAsync() =>
        WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypesAsync(
            WKWebsiteDataStore.AllWebsiteDataTypes, NSDate.DistantPast);
}

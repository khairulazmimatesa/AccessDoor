using UIKit;

namespace AccessDoor.Mobile.Services;

internal static partial class DeviceIdProvider
{
    private static partial string? GetPlatformDeviceId() =>
        UIDevice.CurrentDevice.IdentifierForVendor?.AsString();
}

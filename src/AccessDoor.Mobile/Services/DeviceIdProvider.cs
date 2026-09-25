namespace AccessDoor.Mobile.Services;

internal static partial class DeviceIdProvider
{
    public static string GetDeviceId()
    {
        var id = GetPlatformDeviceId();
        return string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString("N") : id;
    }

    private static partial string? GetPlatformDeviceId();
}

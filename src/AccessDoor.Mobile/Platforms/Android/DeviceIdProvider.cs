using static Android.Provider.Settings;

namespace AccessDoor.Mobile.Services;

internal static partial class DeviceIdProvider
{
    // Build.Serial is no longer readable by apps on Android 10+, so ANDROID_ID is used instead.
    private static partial string? GetPlatformDeviceId() =>
        Secure.GetString(Android.App.Application.Context.ContentResolver, Secure.AndroidId);
}

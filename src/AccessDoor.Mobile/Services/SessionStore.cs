namespace AccessDoor.Mobile.Services;

/// <summary>Persists the device token and the URL of the last successful login.</summary>
public sealed class SessionStore(IPreferences preferences, ISecureStorage secureStorage)
{
    // Same key the Xamarin app used, so upgraded installs keep their registered device token.
    private const string DeviceTokenKey = "my_id";
    // The login URL embeds the user's key, so it lives in the platform keystore/keychain.
    private const string LoggedInUrlKey = "logged_in_url";

    private string? _deviceToken;

    public string DeviceToken => _deviceToken ??= LoadDeviceToken();

    public async Task<Uri?> GetLoggedInUrlAsync()
    {
        // Earlier 2.0 builds kept the URL in plain preferences; drop it.
        preferences.Remove(LoggedInUrlKey);

        try
        {
            var value = await secureStorage.GetAsync(LoggedInUrlKey);
            return Uri.TryCreate(value, UriKind.Absolute, out var uri) ? uri : null;
        }
        catch (Exception)
        {
            // Keystore entries can become unreadable (e.g. after a backup restore); treat as logged out.
            secureStorage.Remove(LoggedInUrlKey);
            return null;
        }
    }

    public Task SetLoggedInUrlAsync(Uri url) => secureStorage.SetAsync(LoggedInUrlKey, url.AbsoluteUri);

    public void ClearLoggedInUrl() => secureStorage.Remove(LoggedInUrlKey);

    private string LoadDeviceToken()
    {
        var id = preferences.Get(DeviceTokenKey, string.Empty);
        if (string.IsNullOrWhiteSpace(id))
        {
            id = DeviceIdProvider.GetDeviceId();
            preferences.Set(DeviceTokenKey, id);
        }
        return id;
    }
}

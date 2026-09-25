namespace AccessDoor.Mobile.Services;

/// <summary>Persists the device token and the URL of the last successful login.</summary>
public sealed class SessionStore(IPreferences preferences)
{
    // Same key the Xamarin app used, so upgraded installs keep their registered device token.
    private const string DeviceTokenKey = "my_id";
    private const string LoggedInUrlKey = "logged_in_url";

    public string DeviceToken
    {
        get
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

    public Uri? LoggedInUrl
    {
        get => Uri.TryCreate(preferences.Get(LoggedInUrlKey, string.Empty), UriKind.Absolute, out var uri) ? uri : null;
        set
        {
            if (value is null)
                preferences.Remove(LoggedInUrlKey);
            else
                preferences.Set(LoggedInUrlKey, value.AbsoluteUri);
        }
    }
}

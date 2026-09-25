namespace AccessDoor.Core;

/// <summary>Credentials entered by the user plus the device token that identifies this machine/phone.</summary>
public sealed record LoginRequest(string BaseUrl, string Username, string Key, string DeviceToken)
{
    public const string DefaultBaseUrl = "http://103.82.228.86:130/";

    /// <summary>Returns a user-facing validation error, or null when the request is valid.</summary>
    public string? Validate()
    {
        if (!Uri.TryCreate(BaseUrl?.Trim(), UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            return "Please enter a valid web URL (http:// or https://).";

        return (string.IsNullOrWhiteSpace(Username), string.IsNullOrWhiteSpace(Key)) switch
        {
            (true, true) => "Please enter your username and key.",
            (true, false) => "Please enter your username.",
            (false, true) => "Please enter your key.",
            _ => null,
        };
    }

    /// <summary>Builds the login URL with every query value properly escaped.</summary>
    public Uri ToUri()
    {
        var builder = new UriBuilder(BaseUrl.Trim())
        {
            Query = $"username={Uri.EscapeDataString(Username.Trim())}" +
                    $"&key={Uri.EscapeDataString(Key.Trim())}" +
                    $"&token={Uri.EscapeDataString(DeviceToken)}",
        };
        return builder.Uri;
    }
}

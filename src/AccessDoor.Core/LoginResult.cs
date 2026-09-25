namespace AccessDoor.Core;

/// <summary>
/// Result the server reports by redirecting to a URL carrying <c>msg</c> and <c>IsAllow</c> query parameters.
/// </summary>
public sealed record LoginResult(bool IsAllowed, string? Message)
{
    /// <summary>
    /// Parses the redirect URL. Returns null when the URL carries no login result
    /// (i.e. an ordinary page navigation, which is treated as allowed).
    /// </summary>
    public static LoginResult? TryParse(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return null;

        var queryStart = url.IndexOf('?');
        if (queryStart < 0)
            return null;

        string? msg = null;
        string? isAllow = null;
        foreach (var pair in url[(queryStart + 1)..].Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);
            var name = Uri.UnescapeDataString(parts[0]);
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1].Replace('+', ' ')) : string.Empty;

            if (name.Equals("msg", StringComparison.OrdinalIgnoreCase))
                msg = value;
            else if (name.Equals("IsAllow", StringComparison.OrdinalIgnoreCase))
                isAllow = value;
        }

        if (msg is null)
            return null;

        return new LoginResult(bool.TryParse(isAllow, out var allowed) && allowed, msg);
    }
}

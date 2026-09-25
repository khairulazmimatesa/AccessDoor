namespace AccessDoor.Mobile.Services;

/// <summary>Clears what the embedded browser keeps between sessions (cookies, web storage, caches).</summary>
internal static partial class BrowserData
{
    public static partial Task ClearAsync();
}

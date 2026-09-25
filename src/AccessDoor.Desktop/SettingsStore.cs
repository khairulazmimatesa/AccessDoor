using System.Security.Cryptography;
using System.Text;

namespace AccessDoor.Desktop;

/// <summary>
/// Remembers the last successful login URL in %LocalAppData%\AccessDoor\session.bin.
/// The URL embeds the user's key, so it is encrypted with DPAPI for the current Windows user.
/// </summary>
internal sealed class SettingsStore
{
    private static readonly string Folder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AccessDoor");

    private readonly string _path = Path.Combine(Folder, "session.bin");

    public Uri? LoggedInUrl { get; set; }

    public SettingsStore()
    {
        // Earlier 2.0 builds stored the URL in plain JSON; remove it.
        TryDelete(Path.Combine(Folder, "settings.json"));

        try
        {
            if (File.Exists(_path))
            {
                var bytes = ProtectedData.Unprotect(File.ReadAllBytes(_path), null, DataProtectionScope.CurrentUser);
                LoggedInUrl = Uri.TryCreate(Encoding.UTF8.GetString(bytes), UriKind.Absolute, out var uri) ? uri : null;
            }
        }
        catch (Exception ex) when (ex is IOException or CryptographicException or UnauthorizedAccessException)
        {
            // A corrupt or unreadable file just means "not logged in".
        }
    }

    public void Save()
    {
        if (LoggedInUrl is null)
        {
            TryDelete(_path);
            return;
        }

        Directory.CreateDirectory(Folder);
        var bytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(LoggedInUrl.AbsoluteUri), null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes(_path, bytes);
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }
}

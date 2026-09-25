using System.Text.Json;

namespace AccessDoor.Desktop;

/// <summary>Remembers the last successful login URL in %LocalAppData%\AccessDoor\settings.json.</summary>
internal sealed class SettingsStore
{
    private readonly string _path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AccessDoor", "settings.json");

    public Uri? LoggedInUrl { get; set; }

    public SettingsStore()
    {
        try
        {
            if (File.Exists(_path) &&
                JsonSerializer.Deserialize<Data>(File.ReadAllText(_path)) is { LoggedInUrl: { } url })
            {
                LoggedInUrl = Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri : null;
            }
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            // A corrupt or unreadable settings file just means "not logged in".
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(new Data(LoggedInUrl?.AbsoluteUri)));
    }

    private sealed record Data(string? LoggedInUrl);
}

using System.Management;
using System.Runtime.Versioning;

namespace AccessDoor.Desktop;

[SupportedOSPlatform("windows")]
internal static class DeviceToken
{
    /// <summary>
    /// The CPU processor ID, lower-cased — the same token the original desktop app sent,
    /// so machines already registered on the server keep working.
    /// </summary>
    public static string Get()
    {
        using var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
        using var results = searcher.Get();

        foreach (var obj in results)
        {
            using (obj)
            {
                if (obj["ProcessorId"] is string id && !string.IsNullOrWhiteSpace(id))
                    return id.Replace(":", "", StringComparison.Ordinal).ToLowerInvariant();
            }
        }

        return Environment.MachineName.ToLowerInvariant();
    }
}

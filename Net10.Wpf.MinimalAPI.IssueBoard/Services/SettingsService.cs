using System.IO;
using System.Text.Json;

namespace Net10.Wpf.MinimalAPI.IssueBoard.Services;

public static class SettingsService
{
    private static readonly string SettingsDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IssueBoard");

    private static readonly string SettingsPath =
        Path.Combine(SettingsDir, "settings.json");

    public static string GetAuthorName()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return string.Empty;
            var json = File.ReadAllText(SettingsPath);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return dict?.TryGetValue("AuthorName", out var name) == true ? name : string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static void SaveAuthorName(string authorName)
    {
        try
        {
            Directory.CreateDirectory(SettingsDir);
            var dict = new Dictionary<string, string> { { "AuthorName", authorName } };
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(dict));
        }
        catch { /* 保存失敗は無視 */ }
    }
}

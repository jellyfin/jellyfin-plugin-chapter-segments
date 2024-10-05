using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.ChapterSegments.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the regular expression pattern for detecting the intro segment type.
    /// </summary>
    public string? IntroPattern { get; set; } = "intro|opening";
}

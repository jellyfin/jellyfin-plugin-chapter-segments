using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jellyfin.Data.Enums;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.ChapterSegments.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the user provided regex text for Intros.
    /// </summary>
    public string? CustomMappingIntro { get; set; } = "intro|opening|^OP$";

    /// <summary>
    /// Gets or sets the user provided regex text for Commercials.
    /// </summary>
    public string? CustomMappingPreview { get; set; } = "preview|next time on|next on|sneak peek";

    /// <summary>
    /// Gets or sets the user provided regex text for Previews.
    /// </summary>
    public string? CustomMappingRecap { get; set; } = "recap|last time on|last on|previously on";

    /// <summary>
    /// Gets or sets the user provided regex text for Recaps.
    /// </summary>
    public string? CustomMappingOutro { get; set; } = "outro|closing|ending|^ED$";

    /// <summary>
    /// Gets or sets the user provided regex text for Outros.
    /// </summary>
    public string? CustomMappingCommercial { get; set; } = "break|ad|advertisement|intermission";

    /// <summary>
    /// Gets the regular expressions with a mapping of their respective Segment types.
    /// </summary>
    /// <returns>A list of regexes with their respective segment types.</returns>
    public IReadOnlyList<(MediaSegmentType Type, string? Regex)> Patterns()
    {
        return
        [
            (MediaSegmentType.Intro, CustomMappingIntro),
            (MediaSegmentType.Commercial, CustomMappingCommercial),
            (MediaSegmentType.Preview, CustomMappingPreview),
            (MediaSegmentType.Recap, CustomMappingRecap),
            (MediaSegmentType.Outro, CustomMappingOutro),
        ];
    }
}

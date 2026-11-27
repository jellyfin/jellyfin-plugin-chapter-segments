using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jellyfin.Data.Enums;
using Jellyfin.Database.Implementations.Enums;
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
    public string? CustomMappingIntro { get; set; } = @"^(intro|opening( (credits|titles|theme|song))?|op|title sequence)( ?\d+)?$";

    /// <summary>
    /// Gets or sets the user provided regex text for Previews.
    /// </summary>
    public string? CustomMappingPreview { get; set; } = @"^((next (episode )?)?previews?|sneak peeks?|next time|next episode)( ?\d+)?$|^next (time )?on\b";

    /// <summary>
    /// Gets or sets the user provided regex text for Recaps.
    /// </summary>
    public string? CustomMappingRecap { get; set; } = @"^((the )?story so far|recaps?|previously|last time)( ?\d+)?$|^(previously|last( time)?) on\b";

    /// <summary>
    /// Gets or sets the user provided regex text for Outros.
    /// </summary>
    public string? CustomMappingOutro { get; set; } = @"^(outro|ending( (credits|titles|theme|song))?|ed|(end|closing) (credits|titles)|credits|closing)( ?\d+)?$";

    /// <summary>
    /// Gets or sets the user provided regex text for Commercials.
    /// </summary>
    public string? CustomMappingCommercial { get; set; } = @"^(ads?|break|(ad|commercial) break|commercials?|advert(isement)?s?|intermission)( ?\d+)?$";

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

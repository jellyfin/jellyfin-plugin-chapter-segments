using System;
using System.Collections.Generic;
using Jellyfin.Plugin.ChapterSegments.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.ChapterSegments;

/// <summary>
/// Plugin entrypoint.
/// </summary>
public class ChapterSegmentsPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly Guid _id = new("698b6f33-14ca-49b5-9d79-fc3c0ab941f5");

    /// <summary>
    /// Initializes a new instance of the <see cref="ChapterSegmentsPlugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public ChapterSegmentsPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static ChapterSegmentsPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public override Guid Id => _id;

    /// <inheritdoc />
    public override string Name => "Chapter Segments Provider";

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages() =>
    [
        new()
        {
            Name = Name,
            EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.config.html"
        }
    ];
}

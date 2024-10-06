using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Persistence;
using MediaBrowser.Model;
using MediaBrowser.Model.MediaSegments;

namespace Jellyfin.Plugin.ChapterSegments;

/// <inheritdoc />
public class ChapterMediaSegmentProvider(IItemRepository itemRepository) : IMediaSegmentProvider
{
    /// <inheritdoc />
    public string Name => "Chapter Segments Provider";

    /// <inheritdoc />
    public ValueTask<bool> Supports(BaseItem item) => new(item is IHasMediaSources);

    private MediaSegmentType? GetMediaSegmentType(string name)
    {
        var mappings = ChapterSegmentsPlugin.Instance?.Configuration.Patterns();

        foreach (var item in mappings!.Where(e => !string.IsNullOrWhiteSpace(e.Regex)))
        {
            if (!string.IsNullOrEmpty(item.Regex)
                && Regex.IsMatch(name, item.Regex, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                return item.Type;
            }
        }

        return null;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<MediaSegmentDto>> GetMediaSegments(MediaSegmentGenerationRequest request, CancellationToken cancellationToken)
    {
        var item = itemRepository.RetrieveItem(request.ItemId);
        if (item is not IHasMediaSources mediaItem)
        {
            return Task.FromResult<IReadOnlyList<MediaSegmentDto>>(Array.Empty<MediaSegmentDto>());
        }

        var chapters = itemRepository.GetChapters(item);
        if (chapters.Count == 0)
        {
            // No chapters, so nothing to parse.
            return Task.FromResult<IReadOnlyList<MediaSegmentDto>>(Array.Empty<MediaSegmentDto>());
        }

        var segments = new List<MediaSegmentDto>(chapters.Count);

        for (var index = 0; index < chapters.Count; index++)
        {
            var chapterInfo = chapters[index];
            var nextChapterInfo = index + 1 < chapters.Count ? chapters[index + 1] : null;

            if (string.IsNullOrEmpty(chapterInfo.Name))
            {
                continue;
            }

            var type = GetMediaSegmentType(chapterInfo.Name);

            if (type.HasValue)
            {
                segments.Add(new MediaSegmentDto
                {
                    Id = Guid.NewGuid(),
                    ItemId = item.Id,
                    Type = type.Value,
                    StartTicks = chapterInfo.StartPositionTicks,
                    EndTicks = nextChapterInfo?.StartPositionTicks ?? mediaItem.RunTimeTicks ?? chapterInfo.StartPositionTicks
                });
            }
        }

        return Task.FromResult<IReadOnlyList<MediaSegmentDto>>(segments);
    }
}

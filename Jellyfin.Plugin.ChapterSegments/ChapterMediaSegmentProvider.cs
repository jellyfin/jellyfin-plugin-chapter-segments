﻿using System;
using System.Collections.Generic;
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
        // TODO This should support all the types
        var introPattern = ChapterSegmentsPlugin.Instance?.Configuration.IntroPattern;
        if (introPattern == null)
        {
            return null;
        }

        var introRegex = new Regex(introPattern);
        return introRegex.IsMatch(name) ? MediaSegmentType.Intro : null;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<MediaSegmentDto>> GetMediaSegments(MediaSegmentGenerationRequest request, CancellationToken cancellationToken)
    {
        var item = itemRepository.RetrieveItem(request.ItemId);
        if (item is not IHasMediaSources mediaItem)
        {
            return Task.FromResult<IReadOnlyList<MediaSegmentDto>>(new List<MediaSegmentDto>());
        }

        var chapters = itemRepository.GetChapters(item);
        var segments = new List<MediaSegmentDto>(chapters.Count);

        for (var index = 0; index < chapters.Count; index++)
        {
            var chapterInfo = chapters[index];
            var nextChapterInfo = index + 1 < chapters.Count ? chapters[index + 1] : null;

            if (chapterInfo.Name == null)
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
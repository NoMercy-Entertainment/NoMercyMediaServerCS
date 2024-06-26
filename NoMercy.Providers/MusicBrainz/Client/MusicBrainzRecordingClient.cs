﻿// ReSharper disable All

using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class MusicBrainzRecordingClient : MusicBrainzBaseClient
{
    public MusicBrainzRecordingClient(Guid? id, string[]? appendices = null) : base((Guid)id!)
    {
    }

    public Task<MusicBrainzRecordingAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        Dictionary<string, string>? queryParams = new()
        {
            ["inc"] = string.Join("+", appendices),
            ["fmt"] = "json"
        };

        return Get<MusicBrainzRecordingAppends>("recording/" + Id, queryParams, priority);
    }

    public Task<MusicBrainzRecordingAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "artist-credits",
            "artists",
            "releases",
            "tags",
            "genres"
        ], priority);
    }
}
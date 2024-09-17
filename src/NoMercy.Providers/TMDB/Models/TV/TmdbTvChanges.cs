﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvChanges
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("items")] public TmdbTvChangeItem[] Items { get; set; } = [];
}
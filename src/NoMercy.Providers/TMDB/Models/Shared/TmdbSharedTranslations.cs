﻿using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbSharedTranslations
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("translations")] public TmdbSharedTranslation[] Translations { get; set; } = [];
}
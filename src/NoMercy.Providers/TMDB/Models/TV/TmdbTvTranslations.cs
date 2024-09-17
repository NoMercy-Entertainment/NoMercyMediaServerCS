﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvTranslations : TmdbSharedTranslations
{
    [JsonProperty("translations")] public new TmdbTvTranslation[] Translations { get; set; } = [];
}
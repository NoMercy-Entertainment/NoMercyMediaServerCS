#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class TmdbCollectionsTranslations : TmdbSharedTranslations
{
    [JsonProperty("translations")] public new TmdbCollectionsTranslation[] Translations { get; set; }
}

public class TmdbCollectionsTranslation : TmdbSharedTranslation
{
    [JsonProperty("data")] public new TmdbCollectionsTranslationData Data { get; set; }
}

public class TmdbCollectionsTranslationData : TmdbSharedTranslationData
{
    [JsonProperty("title")] public string? Title { get; set; }
}
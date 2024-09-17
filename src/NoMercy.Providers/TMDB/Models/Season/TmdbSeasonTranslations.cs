using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Season;

public class TmdbSeasonTranslations : TmdbSharedTranslations
{
    [JsonProperty("translations")] public new TmdbSeasonTranslation[] Translations { get; set; } = [];
}

public class TmdbSeasonTranslation : TmdbSharedTranslation
{
    [JsonProperty("data")] public new TmdbSeasonTranslationData Data { get; set; } = new();
}

public class TmdbSeasonTranslationData : TmdbSharedTranslationData
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}
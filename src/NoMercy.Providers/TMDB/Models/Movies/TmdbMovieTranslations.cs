using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieTranslations : TmdbSharedTranslations
{
    [JsonProperty("translations")] public new TmdbMovieTranslation[] Translations { get; set; } = [];
}

public class TmdbMovieTranslation : TmdbSharedTranslation
{
    [JsonProperty("data")] public new TmdbMovieTranslationData Data { get; set; } = new();
}

public class TmdbMovieTranslationData : TmdbSharedTranslationData
{
    [JsonProperty("title")] public string? Title { get; set; }
}
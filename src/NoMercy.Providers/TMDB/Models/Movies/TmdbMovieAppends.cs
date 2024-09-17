#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieAppends : TmdbMovieDetails
{
    [JsonProperty("alternative_titles")] public TmdbMovieAlternativeTitles AlternativeTitles { get; set; }
    [JsonProperty("credits")] public TmdbMovieCredits Credits { get; set; }
    [JsonProperty("external_ids")] public TmdbMovieExternalIds ExternalIds { get; set; }
    [JsonProperty("images")] public TmdbImages Images { get; set; }
    [JsonProperty("keywords")] public TmdbMovieKeywords Keywords { get; set; }
    [JsonProperty("recommendations")] public TmdbMovieRecommendations Recommendations { get; set; }
    [JsonProperty("similar")] public TmdbMovieSimilar Similar { get; set; }
    [JsonProperty("translations")] public TmdbCombinedTranslations Translations { get; set; }
    [JsonProperty("videos")] public TmdbMovieVideos Videos { get; set; }
    [JsonProperty("watch/providers")] public TmdbMovieWatchProviders WatchProviders { get; set; }
    [JsonProperty("genres")] public new TmdbGenre[] Genres { get; set; } = [];
    [JsonProperty("release_dates")] public TmdbMovieReleaseDates ReleaseDates { get; set; }
}
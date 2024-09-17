using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvRecommendations : TmdbPaginatedResponse<RecommendationsTmdbTvShow>
{
    //
}

public class RecommendationsTmdbTvShow : TmdbTvShow
{
    [JsonProperty("adult")] public bool Adult { get; set; }
}
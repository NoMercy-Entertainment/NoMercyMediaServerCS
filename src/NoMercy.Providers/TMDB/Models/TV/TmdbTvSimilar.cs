using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvSimilar : TmdbPaginatedResponse<TmdbSimilarTmdbTvShow>
{
    //
}

public class TmdbSimilarTmdbTvShow : TmdbTvShow
{
    [JsonProperty("adult")] public bool Adult { get; set; }
}
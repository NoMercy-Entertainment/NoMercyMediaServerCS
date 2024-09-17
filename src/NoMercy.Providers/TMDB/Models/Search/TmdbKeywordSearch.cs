using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Search;

public class TmdbKeywordSearch : TmdbPaginatedResponse<TmdbKeywordSearchResult>
{
}

public class TmdbKeywordSearchResult
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}
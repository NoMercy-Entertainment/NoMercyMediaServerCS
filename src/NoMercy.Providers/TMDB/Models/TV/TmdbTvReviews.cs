#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvReviews : TmdbPaginatedResponse<ReviewsResult>
{
    [JsonProperty("id")] public int Id { get; set; }
}
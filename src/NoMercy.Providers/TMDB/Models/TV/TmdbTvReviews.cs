#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvReviews : TmdbPaginatedResponse<ReviewsResult>
{
    [JsonProperty("id")] public int Id { get; set; }
}

public class ReviewsResult
{
    [JsonProperty("author")] public string Author { get; set; }
    [JsonProperty("author_details")] public TmdbAuthorDetails TmdbAuthorDetails { get; set; }
    [JsonProperty("content")] public string Content { get; set; }
    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
    [JsonProperty("url")] public Uri Url { get; set; }
}

public class TmdbAuthorDetails
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("avatar_path")] public string AvatarPath { get; set; }
    [JsonProperty("rating")] public int Rating { get; set; }
}
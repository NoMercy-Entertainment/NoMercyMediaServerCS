using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;
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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class TmdbPersonCredit
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("backdrop_path")] public string BackdropPath { get; set; }
    [JsonProperty("character")] public string? Character { get; set; }
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("department")] public string Department { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
    [JsonProperty("first_air_date")] public DateTime? FirstAirDate { get; set; }
    [JsonProperty("genre_ids")] public int[] GenreIds { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("job")] public string? Job { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("order")] public int Order { get; set; }
    [JsonProperty("origin_country")] public string[] OriginCountry { get; set; }
    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("poster_path")] public string PosterPath { get; set; }
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
    [JsonProperty("vote_average")] public double VoteAverage { get; set; }
    [JsonProperty("vote_count")] public int VoteCount { get; set; }
}
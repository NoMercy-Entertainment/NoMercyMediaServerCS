using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbKnownFor
{
    [JsonProperty("poster_path")] public string PosterPath { get; set; }
    [JsonProperty("adult")] public bool? Adult { get; set; }
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }
    [JsonProperty("genre_ids")] public int[] GenreIds { get; set; } = [];
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("backdrop_path")] public string BackdropPath { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("vote_count")] public int VoteCount { get; set; }
    [JsonProperty("video")] public bool? Video { get; set; }
    [JsonProperty("vote_average")] public float VoteAverage { get; set; }
    [JsonProperty("first_air_date")] public DateTime? FirstAirDate { get; set; }
    [JsonProperty("origin_country")] public string[] OriginCountry { get; set; } = [];
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }
}
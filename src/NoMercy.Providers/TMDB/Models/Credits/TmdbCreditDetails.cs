#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Credits;

public class TmdbCreditDetails
{
    [JsonProperty("credit_type")] public string CreditType { get; set; } = string.Empty;
    [JsonProperty("department")] public string Department { get; set; } = string.Empty;
    [JsonProperty("job")] public string Job { get; set; } = string.Empty;
    [JsonProperty("media")] public TmdbMedia? Media { get; set; }
    [JsonProperty("media_type")] public string? MediaType { get; set; }
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;
    [JsonProperty("person")] public TmdbPerson TmdbPerson { get; set; } = new();
}

public class TmdbMedia
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("original_name")] public string OriginalName { get; set; } = string.Empty;
    [JsonProperty("character")] public string Character { get; set; } = string.Empty;
    [JsonProperty("episodes")] public Episode.TmdbEpisode[] Episodes { get; set; } = [];
    [JsonProperty("seasons")] public Season.TmdbSeason[] Seasons { get; set; } = [];
}

public class S : Season.TmdbSeason
{
    [JsonProperty("air_date")] public new DateTime AirDate { get; set; }
    [JsonProperty("poster_path")] public new string? PosterPath { get; set; }
    [JsonProperty("season_number")] public new int SeasonNumber { get; set; }
}

public class TmdbPerson
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
}
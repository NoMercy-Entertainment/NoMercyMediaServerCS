using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieDetails : TmdbMovie
{
    [JsonProperty("budget")] public int Budget { get; set; }
    [JsonProperty("genres")] public TmdbGenre[] Genres { get; set; } = [];
    [JsonProperty("homepage")] public Uri? Homepage { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("revenue")] public long Revenue { get; set; }
    [JsonProperty("runtime")] public int Runtime { get; set; }
    [JsonProperty("status")] public string? Status { get; set; }
    [JsonProperty("production_companies")] public TmdbProductionCompany[] ProductionCompanies { get; set; } = [];

    [JsonProperty("belongs_to_collection")]
    public BelongsToCollection? BelongsToCollection { get; set; }

    [JsonProperty("production_countries")] public TmdbProductionCountry[] ProductionCountries { get; set; } = [];
    [JsonProperty("spoken_languages")] public TmdbSpokenLanguage[] SpokenLanguages { get; set; } = [];
}

public class BelongsToCollection
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("poster_path")] public string? PosterPath { get; set; }
    [JsonProperty("backdrop_path")] public string? BackdropPath { get; set; }
}
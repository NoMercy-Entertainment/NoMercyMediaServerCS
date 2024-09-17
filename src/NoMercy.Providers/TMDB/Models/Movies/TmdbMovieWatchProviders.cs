using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieWatchProviders
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbCountries<TmdbPaymentType> Results { get; set; } = new();
}
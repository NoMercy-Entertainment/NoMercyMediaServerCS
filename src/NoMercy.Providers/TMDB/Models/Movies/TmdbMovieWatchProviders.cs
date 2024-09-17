using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieWatchProviders
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbCountries<TmdbPaymentType> Results { get; set; } = new();
}

public class TmdbPaymentType
{
    [JsonProperty("link")] public Uri? Link { get; set; }
    [JsonProperty("flatrate")] public TmdbPaymentDetails[] FlatRate { get; set; } = [];
    [JsonProperty("rent")] public TmdbPaymentDetails[] Rent { get; set; } = [];
    [JsonProperty("buy")] public TmdbPaymentDetails[] Buy { get; set; } = [];
}

public class TmdbPaymentDetails
{
    [JsonProperty("display_priority")] public int DisplayPriority { get; set; }
    [JsonProperty("logo_path")] public string LogoPath { get; set; } = string.Empty;
    [JsonProperty("provider_id")] public int ProviderId { get; set; }
    [JsonProperty("provider_name")] public string ProviderName { get; set; } = string.Empty;
}
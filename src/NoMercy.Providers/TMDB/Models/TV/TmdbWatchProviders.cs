#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbWatchProviders
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbTvWatchProviderResults TmdbTvWatchProviderResults { get; set; }
}

public class TmdbTvWatchProviderResults
{
    [JsonProperty("AR")] public TmdbTvWatchProviderType Ar { get; set; }
    [JsonProperty("AT")] public TmdbTvWatchProviderType At { get; set; }
    [JsonProperty("AU")] public TmdbTvWatchProviderType Au { get; set; }
    [JsonProperty("BE")] public TmdbTvWatchProviderType Be { get; set; }
    [JsonProperty("BR")] public TmdbTvWatchProviderType Br { get; set; }
    [JsonProperty("CA")] public TmdbTvWatchProviderType Ca { get; set; }
    [JsonProperty("CH")] public TmdbTvWatchProviderType Ch { get; set; }
    [JsonProperty("CL")] public TmdbTvWatchProviderType Cl { get; set; }
    [JsonProperty("CO")] public TmdbTvWatchProviderType Co { get; set; }
    [JsonProperty("CZ")] public TmdbTvWatchProviderType Cz { get; set; }
    [JsonProperty("DE")] public TmdbTvWatchProviderType De { get; set; }
    [JsonProperty("DK")] public TmdbTvWatchProviderType Dk { get; set; }
    [JsonProperty("EC")] public TmdbTvWatchProviderType Ec { get; set; }
    [JsonProperty("ES")] public TmdbTvWatchProviderType Es { get; set; }
    [JsonProperty("FI")] public TmdbTvWatchProviderType Fi { get; set; }
    [JsonProperty("FR")] public TmdbTvWatchProviderType Fr { get; set; }
    [JsonProperty("GB")] public TmdbTvWatchProviderType Gb { get; set; }
    [JsonProperty("HU")] public TmdbTvWatchProviderType Hu { get; set; }
    [JsonProperty("IE")] public TmdbTvWatchProviderType Ie { get; set; }
    [JsonProperty("IN")] public TmdbTvWatchProviderType In { get; set; }
    [JsonProperty("IT")] public TmdbTvWatchProviderType It { get; set; }
    [JsonProperty("JP")] public TmdbTvWatchProviderType Jp { get; set; }
    [JsonProperty("MX")] public TmdbTvWatchProviderType Mx { get; set; }
    [JsonProperty("NL")] public TmdbTvWatchProviderType Nl { get; set; }
    [JsonProperty("NO")] public TmdbTvWatchProviderType No { get; set; }
    [JsonProperty("NZ")] public TmdbTvWatchProviderType Nz { get; set; }
    [JsonProperty("PE")] public TmdbTvWatchProviderType Pe { get; set; }
    [JsonProperty("PL")] public TmdbTvWatchProviderType Pl { get; set; }
    [JsonProperty("PT")] public TmdbTvWatchProviderType Pt { get; set; }
    [JsonProperty("RO")] public TmdbTvWatchProviderType Ro { get; set; }
    [JsonProperty("RU")] public TmdbTvWatchProviderType Ru { get; set; }
    [JsonProperty("SE")] public TmdbTvWatchProviderType Se { get; set; }
    [JsonProperty("TR")] public TmdbTvWatchProviderType Tr { get; set; }
    [JsonProperty("US")] public TmdbTvWatchProviderType Us { get; set; }
    [JsonProperty("VE")] public TmdbTvWatchProviderType Ve { get; set; }
    [JsonProperty("ZA")] public TmdbTvWatchProviderType Za { get; set; }
}

public class TmdbTvWatchProviderType
{
    [JsonProperty("link")] public Uri Link { get; set; }
    [JsonProperty("buy")] public TmdbTvWatchProviderTypeData[] Buy { get; set; } = [];
    [JsonProperty("flatrate")] public TmdbTvWatchProviderTypeData[] Flatrate { get; set; } = [];
    [JsonProperty("ads")] public TmdbTvWatchProviderTypeData[] Ads { get; set; } = [];
    [JsonProperty("rent")] public TmdbTvWatchProviderTypeData[] Rent { get; set; } = [];
    [JsonProperty("free")] public TmdbTvWatchProviderTypeData[] Free { get; set; } = [];
}

public class TmdbTvWatchProviderTypeData
{
    [JsonProperty("display_priority")] public int DisplayPriority { get; set; }
    [JsonProperty("logo_path")] public string LogoPath { get; set; }
    [JsonProperty("provider_id")] public int ProviderId { get; set; }
    [JsonProperty("provider_name")] public string ProviderName { get; set; }
}
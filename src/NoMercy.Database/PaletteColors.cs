using Newtonsoft.Json;

namespace NoMercy.Database;
public class PaletteColors
{
    [JsonProperty("dominant", NullValueHandling = NullValueHandling.Ignore)]
    public string Dominant { get; set; }

    [JsonProperty("primary", NullValueHandling = NullValueHandling.Ignore)]
    public string Primary { get; set; }

    [JsonProperty("lightVibrant", NullValueHandling = NullValueHandling.Ignore)]
    public string LightVibrant { get; set; }

    [JsonProperty("darkVibrant", NullValueHandling = NullValueHandling.Ignore)]
    public string DarkVibrant { get; set; }

    [JsonProperty("lightMuted", NullValueHandling = NullValueHandling.Ignore)]
    public string LightMuted { get; set; }

    [JsonProperty("darkMuted", NullValueHandling = NullValueHandling.Ignore)]
    public string DarkMuted { get; set; }

    // private string _dominant;
    // private string _primary;
    // private string _lightVibrant;
    // private string _darkVibrant;
    // private string _lightMuted;
    // private string _darkMuted;
    //
    // [JsonProperty("dominant", NullValueHandling = NullValueHandling.Ignore)]
    // public string Dominant
    // {
    //     get => !_dominant.Contains('#') ? $"#{_dominant}" : _dominant;
    //     set => _dominant = value.Contains('#') ? $"#{value}" : value;
    // }
    //
    // [JsonProperty("primary", NullValueHandling = NullValueHandling.Ignore)]
    // public string Primary
    // {
    //     get => !_primary.Contains('#') ? $"#{_primary}" : _primary;
    //     set => _primary = value.Contains('#') ? $"#{value}" : value;
    // }
    //
    // [JsonProperty("lightVibrant", NullValueHandling = NullValueHandling.Ignore)]
    // public string LightVibrant
    // {
    //     get => !_lightVibrant.Contains('#') ? $"#{_lightVibrant}" : _lightVibrant;
    //     set => _lightVibrant = value.Contains('#') ? $"#{value}" : value;
    // }
    //
    // [JsonProperty("darkVibrant", NullValueHandling = NullValueHandling.Ignore)]
    // public string DarkVibrant
    // {
    //     get => !_darkVibrant.Contains('#') ? $"#{_darkVibrant}" : _darkVibrant;
    //     set => _darkVibrant = value.Contains('#') ? $"#{value}" : value;
    // }
    //
    // [JsonProperty("lightMuted", NullValueHandling = NullValueHandling.Ignore)]
    // public string LightMuted
    // {
    //     get => !_lightMuted.Contains('#') ? $"#{_lightMuted}" : _lightMuted;
    //     set => _lightMuted = value.Contains('#') ? $"#{value}" : value;
    // }
    //
    // [JsonProperty("darkMuted", NullValueHandling = NullValueHandling.Ignore)]
    // public string DarkMuted
    // {
    //     get => !_darkMuted.Contains('#') ? $"#{_darkMuted}" : _darkMuted;
    //     set => _darkMuted = value.Contains('#') ? $"#{value}" : value;
    // }
}
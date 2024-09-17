using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;
public class ReleaseEvent
{
    [JsonProperty("area")] public MusicBrainzArea MusicBrainzArea { get; set; }

    [JsonProperty("date")] private string _date { get; set; }

    [JsonProperty("dateTime")]
    public DateTime? DateTime
    {
        get => !string.IsNullOrWhiteSpace(_date) && !string.IsNullOrEmpty(_date) && _date.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _date = value.ToString() ?? string.Empty;
    }
}
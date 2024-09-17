using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class RecordingArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("artist")] public PurpleArtist Artist { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
}
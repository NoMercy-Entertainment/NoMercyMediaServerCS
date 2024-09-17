using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("artist")] public MusicBrainzArtist MusicBrainzArtist { get; set; }
}
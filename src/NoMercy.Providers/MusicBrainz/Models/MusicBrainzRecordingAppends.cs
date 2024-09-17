using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzRecordingAppends : MusicBrainzRecording
{
    [JsonProperty("first-release-date")] private string? _firstReleaseDate { get; set; }

    public DateTime? FirstReleaseDate
    {
        get => !string.IsNullOrWhiteSpace(_firstReleaseDate) && !string.IsNullOrEmpty(_firstReleaseDate) && _firstReleaseDate.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _firstReleaseDate = value.ToString();
    }

    [JsonProperty("media")] public MusicBrainzMedia[] Media { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("releases")] public MusicBrainzRelease[] Releases { get; set; }
    [JsonProperty("artist-credit")] public MusicBrainzArtistCredit[] ArtistCredit { get; set; }
}
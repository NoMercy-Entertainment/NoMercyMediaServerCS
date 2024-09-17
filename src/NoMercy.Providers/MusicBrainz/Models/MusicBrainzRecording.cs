#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzRecording
{
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("genres")] public object[] Genres { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}

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

public class MusicBrainzArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("artist")] public MusicBrainzArtist MusicBrainzArtist { get; set; }
}
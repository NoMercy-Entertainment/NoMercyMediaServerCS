using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzArtist
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("genres")] public MusicBrainzGenreDetails[]? Genres { get; set; }
    [JsonProperty("iso-3166-1-codes")] public string[] Iso31661Codes { get; set; }
    [JsonProperty("iso-3166-2-codes")] public string[] Iso31662Codes { get; set; }
}

public class MusicBrainzArtistDetails : MusicBrainzArtist
{
    [JsonProperty("isnis")] public string[] Isnis { get; set; }
    [JsonProperty("end_area")] public object ArtistAppendsEndArea { get; set; }
    [JsonProperty("gender-id")] public Guid GenderId { get; set; }
    [JsonProperty("area")] public MusicBrainzArea MusicBrainzArea { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("works")] public MusicBrainzWork[] Works { get; set; }
    [JsonProperty("releases")] public MusicBrainzRelease[] Releases { get; set; }
    [JsonProperty("release-groups")] public MusicBrainzReleaseGroup[] ReleaseGroups { get; set; }
    [JsonProperty("end-area")] public MusicBrainzArea? EndArea { get; set; }
    [JsonProperty("life-span")] public MusicBrainzLifeSpan? LifeSpan { get; set; }
    [JsonProperty("begin-area")] public MusicBrainzArea? BeginArea { get; set; }
    [JsonProperty("ipis")] public string[] Ipis { get; set; }
}

public class MusicBrainzArtistAppends : MusicBrainzArtistDetails
{
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("recordings")] public MusicBrainzRecording[] Recordings { get; set; }
}

public class MusicBrainzArea
{
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("type")] public object Type { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("iso-3166-1-codes")] public string[] Iso31661Codes { get; set; }
    [JsonProperty("iso-3166-2-codes")] public string[] Iso31662Codes { get; set; }
}

public class MusicBrainzLifeSpan
{
    [JsonProperty("begin")] private string? _beginSpan { get; set; }

    public DateTime? BeginDate {
        get => !string.IsNullOrWhiteSpace(_beginSpan) && !string.IsNullOrEmpty(_beginSpan) && _beginSpan.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _beginSpan = value.ToString();
    }

    [JsonProperty("end")] private string? _endSpan { get; set; }

    public DateTime? EndDate
    {
        get => !string.IsNullOrWhiteSpace(_endSpan) && !string.IsNullOrEmpty(_endSpan) && _endSpan.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _endSpan = value.ToString();
    }

    [JsonProperty("ended")] public bool Ended { get; set; }
}
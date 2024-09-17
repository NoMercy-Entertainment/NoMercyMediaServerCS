#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzRelease
{
    [JsonProperty("barcode")] public string Barcode { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("score")] public int? Score { get; set; }

    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }

    // [JsonProperty("genres")] public object[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("media")] public MusicBrainzMedia[] Media { get; set; }
    [JsonProperty("packaging")] public string Packaging { get; set; }
    [JsonProperty("packaging-id")] public Guid? PackagingId { get; set; }
    [JsonProperty("quality")] public string Quality { get; set; }
    [JsonProperty("release-events")] public ReleaseEvent[]? ReleaseEvents { get; set; }
    [JsonProperty("release-group")] public MusicBrainzReleaseGroup MusicBrainzReleaseGroup { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("status-id")] public Guid? StatusId { get; set; }

    [JsonProperty("artist-credit")] public ReleaseArtistCredit[] ArtistCredit { get; set; }
    
    [JsonProperty("text-representation")]
    public MusicBrainzTextRepresentation MusicBrainzTextRepresentation { get; set; }

    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("area")] public MusicBrainzArea MusicBrainzArea { get; set; }

    [JsonProperty("date")] private string _date { get; set; }

    [JsonProperty("dateTime")]
    public DateTime? DateTime
    {
        get => !string.IsNullOrWhiteSpace(_date) && !string.IsNullOrEmpty(_date) && _date.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _date = value.ToString() ?? string.Empty;
    }
}
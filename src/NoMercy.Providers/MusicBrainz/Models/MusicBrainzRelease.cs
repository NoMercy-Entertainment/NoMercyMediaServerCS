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

public class MusicBrainzReleaseAppends : MusicBrainzRelease
{
    // [JsonProperty("aliases")] public object[] Aliases { get; set; }
    // [JsonProperty("annotation")] public object Annotation { get; set; }

    // [JsonProperty("asin")] public object Asin { get; set; }
    [JsonProperty("collections")] public Collection[] Collections { get; set; }
    [JsonProperty("cover-art-archive")] public CoverArtArchive CoverArtArchive { get; set; }
    [JsonProperty("label-info")] public LabelInfo[] LabelInfo { get; set; }
    [JsonProperty("relations")] public MusicBrainzWorkRelation[] Relations { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("genres")] public MusicBrainzGenreDetails[] Genres { get; set; }
}

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

public class ReleaseArtistCredit
{
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("artist")] public MusicBrainzArtistDetails MusicBrainzArtist { get; set; }
}

public class Alias : MusicBrainzLifeSpan
{
    [JsonProperty("locale")] public string? Locale { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("primary")] public bool? Primary { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class Collection
{
    [JsonProperty("editor")] public string Editor { get; set; }
    [JsonProperty("entity-type")] public string EntityType { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("release-count")] public int ReleaseCount { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class CoverArtArchive
{
    [JsonProperty("artwork")] public bool Artwork { get; set; }
    [JsonProperty("back")] public bool Back { get; set; }
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("darkened")] public bool Darkened { get; set; }
    [JsonProperty("front")] public bool Front { get; set; }
}

public class LabelInfo
{
    [JsonProperty("catalog-number")] public string CatalogNumber { get; set; }
    [JsonProperty("label")] public MusicBrainzLabel MusicBrainzLabel { get; set; }
}

public class MusicBrainzLabel
{
    [JsonProperty("aliases")] public Alias[] Aliases { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("genres")] public MusicBrainzGenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("label-code")] public string? LabelCode { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class Disc
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("offset-count")] public int OffsetCount { get; set; }
    [JsonProperty("offsets")] public int[] Offsets { get; set; }
    [JsonProperty("sectors")] public int Sectors { get; set; }
}

public class MusicBrainzTrack
{
    private int _number;
    [JsonProperty("artist-credit")] public ReleaseArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    
    /** Track length in milliseconds */
    [JsonProperty("length")] public int? Length { get; set; }
    public double Duration => (Length ?? 0) / 1000.0;

    [JsonProperty("number")]
    public int Number
    {
        get => _number;
        set
        {
            try
            {
                _number = Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                _number = value.ToString().Replace("A", "").Split("-").LastOrDefault()?.ToInt() ?? 0;
            }
        }
    }

    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("recording")] public TrackRecording Recording { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("genres")] public MusicBrainzGenreDetails[]? Genres { get; set; }
}

public class TrackRecording
{
    [JsonProperty("aliases")] public Alias[] Aliases { get; set; }
    [JsonProperty("artist-credit")] public RecordingArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }

    [JsonProperty("first-release-date")] private string? _firstReleaseDate { get; set; }

    public DateTime? FirstReleaseDate
    {
        get => !string.IsNullOrWhiteSpace(_firstReleaseDate) && !string.IsNullOrEmpty(_firstReleaseDate) && _firstReleaseDate.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _firstReleaseDate = value.ToString() ?? string.Empty;
    }

    [JsonProperty("genres")] public MusicBrainzGenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("isrcs")] public string[] Isrcs { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("relations")] public RecordingRelation[] Relations { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
}

public class RecordingArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("artist")] public PurpleArtist Artist { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
}

public class PurpleArtist
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }

    [JsonProperty("id")] public Guid Id { get; set; }

    // [JsonProperty("label-code")] public object LabelCode { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }

    [JsonProperty("iso-3166-1-codes")] public string[] Iso31661Codes { get; set; }
}

public class RecordingRelation : MusicBrainzLifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public MusicBrainzAttributeValues MusicBrainzAttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("artist")] public PurpleArtist Artist { get; set; }
    [JsonProperty("attribute-credits")] public AttributeCredits AttributeCredits { get; set; }
    [JsonProperty("label")] public PurpleArtist Label { get; set; }
    [JsonProperty("work")] public MusicBrainzWork MusicBrainzWork { get; set; }
    [JsonProperty("recording")] public RelationRecording Recording { get; set; }
}

public class AttributeCredits
{
    [JsonProperty("Rhodes piano")] public string RhodesPiano { get; set; }
    [JsonProperty("synthesizer")] public string Synthesizer { get; set; }
    [JsonProperty("drums (drum set)")] public string? DrumsDrumSet { get; set; }
    [JsonProperty("handclaps")] public string Handclaps { get; set; }
    [JsonProperty("Hammond organ")] public string HammondOrgan { get; set; }
    [JsonProperty("keyboard")] public string Keyboard { get; set; }
    [JsonProperty("drum machine")] public string DrumMachine { get; set; }
    [JsonProperty("foot stomps")] public string FootStomps { get; set; }

    [JsonProperty("Wurlitzer electric piano")]
    public string WurlitzerElectricPiano { get; set; }
}

public class MusicBrainzAttributeValues
{
    [JsonProperty("task")] public string Task { get; set; }
}

public class RelationRecording
{
    [JsonProperty("artist-credit")] public RecordingArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }

    [JsonProperty("id")] public Guid Id { get; set; }

    // [JsonProperty("isrcs")] public object[] Isrcs { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
}

public class MusicBrainzWorkRelation : MusicBrainzLifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public MusicBrainzAttributeValues MusicBrainzAttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("artist")] public MusicBrainzArtist MusicBrainzArtist { get; set; }
    [JsonProperty("label")] public MusicBrainzLabel MusicBrainzLabel { get; set; }
    [JsonProperty("url")] public MusicBrainzUrl MusicBrainzUrl { get; set; }
}

public class MusicBrainzUrl
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("resource")] public Uri Resource { get; set; }
}

public class MusicBrainzReleaseGroup
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }

    [JsonProperty("first-release-date")] private string? _firstReleaseDate { get; set; }

    public DateTime? FirstReleaseDate
    {
        get => !string.IsNullOrWhiteSpace(_firstReleaseDate) && !string.IsNullOrEmpty(_firstReleaseDate) && _firstReleaseDate.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _firstReleaseDate = value.ToString() ?? string.Empty;
    }

    [JsonProperty("genres")] public MusicBrainzGenreDetails[]? Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("primary-type")] public string PrimaryType { get; set; }
    [JsonProperty("primary-type-id")] public Guid? PrimaryTypeId { get; set; }
    [JsonProperty("secondary-type-ids")] public Guid[] SecondaryTypeIds { get; set; }
    [JsonProperty("secondary-types")] public string[] SecondaryTypes { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}

public class MusicBrainzReleaseGroupDetails : MusicBrainzReleaseGroup
{
    [JsonProperty("releases")] public MusicBrainzRelease[] Releases { get; set; }
    [JsonProperty("relations")] public MusicBrainzWorkRelation[] Relations { get; set; }
}

public class MusicBrainzReleaseSearchResponse
{
    [JsonProperty("created")]
    public DateTimeOffset Created { get; set; }

    [JsonProperty("count")]
    public long Count { get; set; }

    [JsonProperty("offset")]
    public long Offset { get; set; }

    [JsonProperty("releases")]
    public MusicBrainzRelease[] Releases { get; set; }
}
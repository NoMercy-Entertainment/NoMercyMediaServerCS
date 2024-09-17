#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.AcoustId.Models;

public class AcoustIdFingerprint
{
    [JsonProperty("results")] public AcoustIdFingerprintResult[] Results { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
}

public class AcoustIdFingerprintResult
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("recordings")] public AcoustIdFingerprintRecording?[]? Recordings { get; set; }
    [JsonProperty("score")] public double Score { get; set; }
}

public class AcoustIdFingerprintRecording
{
    [JsonProperty("artists")] public AcoustIdFingerprintArtist[] Artists { get; set; }
    [JsonProperty("duration")] public int Duration { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("releases")] public AcoustIdFingerprintReleaseGroups[]? Releases { get; set; }
    [JsonProperty("sources")] public int Sources { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
}

public class AcoustIdFingerprintReleaseGroups
{
    [JsonProperty("artists")] public AcoustIdFingerprintArtist[] Artists { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("date")] public AcoustIdFingerprintDate? Date { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("medium_count")] public int? MediumCount { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("track_count")] public int? TrackCount { get; set; } = 0;

    [JsonProperty("mediums")] public AcoustIdFingerprintMedium[] Mediums { get; set; }
    [JsonProperty("releaseevents")] public AcoustIdFingerprintReleaseEvent[] Releaseevents { get; set; }
}

public class AcoustIdFingerprintMedium
{
    [JsonProperty("format")] public string? Format { get; set; }
    [JsonProperty("position")] public int? Position { get; set; }
    [JsonProperty("track_count")] public int? TrackCount { get; set; }
    [JsonProperty("tracks")] public AcoustIdFingerprintTrack[] Tracks { get; set; }
}

public class AcoustIdFingerprintTrack
{
    [JsonProperty("artists")] public AcoustIdFingerprintArtist[]? Artists { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("position")] public int? Position { get; set; }
}

public class AcoustIdFingerprintArtist
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}

public class AcoustIdFingerprintReleaseEvent
{
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("date")] public AcoustIdFingerprintDate Date { get; set; }
}

public class AcoustIdFingerprintDate
{
    [JsonProperty("day")] public int Day { get; set; }
    [JsonProperty("month")] public int Month { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
}
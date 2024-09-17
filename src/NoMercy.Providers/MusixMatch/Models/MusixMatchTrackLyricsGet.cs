#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchTrackLyricsGet
{
    [JsonProperty("message")] public TrackLyricsGetMessage? Message { get; set; }
}

public class TrackLyricsGetMessage
{
    [JsonProperty("header")] public TrackLyricsGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public TrackLyricsGetMessagedBody? Body { get; set; }
}

public class TrackLyricsGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
}

public class TrackLyricsGetMessagedBody
{
    [JsonProperty("lyrics")] public MusixMatchLyrics? Lyrics { get; set; }
}
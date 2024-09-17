#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchMatcherTrackGet
{
    [JsonProperty("message")] public MatcherTrackGetMessage Message { get; set; }
}

public class MatcherTrackGetMessage
{
    [JsonProperty("header")] public MusixMatchMatcherTrackGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public MatcherTrackGetMessageBody Body { get; set; }
}

public class MatcherTrackGetMessageBody
{
    [JsonProperty("track")] public MusixMatchMusixMatchTrack MusixMatchMusixMatchTrack { get; set; }
}

public class MusixMatchMatcherTrackGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
    [JsonProperty("confidence")] public long Confidence { get; set; }
    [JsonProperty("mode")] public string Mode { get; set; }
    [JsonProperty("cached")] public long Cached { get; set; }
}
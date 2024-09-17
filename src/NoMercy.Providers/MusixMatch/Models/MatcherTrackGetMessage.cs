using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;
public class MatcherTrackGetMessage
{
    [JsonProperty("header")] public MusixMatchMatcherTrackGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public MatcherTrackGetMessageBody Body { get; set; }
}
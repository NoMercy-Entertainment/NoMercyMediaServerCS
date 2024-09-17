using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;
public class TrackSnippetGetMessage
{
    [JsonProperty("header")] public TrackSnippetGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public TrackSnippetGetMessageBody Body { get; set; }
}
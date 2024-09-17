#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchSubtitleGet
{
    [JsonProperty("message")] public MusixMatchSubtitleGetMessage? Message { get; set; }
}

public class MusixMatchSubtitleGetMessage
{
    [JsonProperty("header")] public MusixMatchSubtitleGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public MusixMatchSubtitleGetMessageBody? Body { get; set; }
}

public class MusixMatchSubtitleGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
    [JsonProperty("pid")] public long Pid { get; set; }
    [JsonProperty("surrogate_key_list")] public object[] SurrogateKeyList { get; set; }
}

public class MusixMatchSubtitleGetMessageBody
{
    [JsonProperty("macro_calls")] public MusixMatchMacroCalls? MacroCalls { get; set; }
}
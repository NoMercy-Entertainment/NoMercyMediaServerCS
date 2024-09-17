#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchRichSync
{
    [JsonProperty("richsync_id")] public int RichsyncId;
    [JsonProperty("restricted")] public int Restricted;
    [JsonProperty("richsync_body")] public string RichsyncBody;
    [JsonProperty("lyrics_copyright")] public string LyricsCopyright;
    [JsonProperty("richsync_length")] public int RichsyncLength;
    [JsonProperty("richsync_language")] public string RichsyncLanguage;

    [JsonProperty("richsync_language_description")]
    public string RichsyncLanguageDescription;

    [JsonProperty("script_tracking_url")] public string ScriptTrackingUrl;
    [JsonProperty("updated_time")] public DateTime UpdatedTime;
}
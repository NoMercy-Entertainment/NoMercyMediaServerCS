#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchSnippet
{
    [JsonProperty("snippet_id")] public long SnippetId { get; set; }
    [JsonProperty("snippet_language")] public string SnippetLanguage { get; set; }
    [JsonProperty("restricted")] public long Restricted { get; set; }
    [JsonProperty("instrumental")] public long Instrumental { get; set; }
    [JsonProperty("snippet_body")] public string SnippetBody { get; set; }
    [JsonProperty("script_tracking_url")] public Uri ScriptTrackingUrl { get; set; }
    [JsonProperty("pixel_tracking_url")] public Uri PixelTrackingUrl { get; set; }
    [JsonProperty("html_tracking_url")] public Uri HtmlTrackingUrl { get; set; }
    [JsonProperty("updated_time")] public DateTimeOffset UpdatedTime { get; set; }
}
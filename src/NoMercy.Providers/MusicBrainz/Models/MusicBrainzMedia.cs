using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzMedia
{
    [JsonProperty("track-count")] public int TrackCount { get; set; }
    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("format")] public string Format { get; set; }
    [JsonProperty("format-id")] public Guid? FormatId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("tracks")] public MusicBrainzTrack[] Tracks { get; set; }
}
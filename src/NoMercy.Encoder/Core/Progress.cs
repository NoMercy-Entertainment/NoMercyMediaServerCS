using Newtonsoft.Json;

namespace NoMercy.Encoder.Core;

public class ProgressMeta
{
    [JsonProperty("has_gpu")] public bool HasGpu { get; set; }
    [JsonProperty("is_hdr")] public bool IsHDR { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("base_folder")] public string BaseFolder { get; set; }
    [JsonProperty("share_path")] public string ShareBasePath { get; set; }
    [JsonProperty("video_streams")] public List<string> VideoStreams { get; set; }
    [JsonProperty("audio_streams")] public List<string> AudioStreams { get; set; }
    [JsonProperty("subtitle_streams")] public List<string> SubtitleStreams { get; set; }
}

public class Progress : ProgressMeta
{
    [JsonProperty("frame")] public int Frame { get; set; }
    [JsonProperty("fps")] public double Fps { get; set; }
    [JsonProperty("bitrate")] public string Bitrate { get; set; }
    [JsonProperty("progress")] public double Percentage { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("speed")] public double Speed { get; set; }
    [JsonProperty("duration")] public double Duration { get; set; }
    [JsonProperty("remaining")] public double Remaining { get; set; }
    [JsonProperty("remaining_hms")] public string RemainingHms { get; set; }
    [JsonProperty("remaining_split")] public string[] RemainingSplit { get; set; }
    [JsonProperty("current_time")] public double CurrentTime { get; set; }
    [JsonProperty("thumbnails")] public string Thumbnail { get; set; }

    public Progress()
    {
        //
    }
}
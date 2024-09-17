using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.Socket;
public class Lyric
{
    [JsonProperty("text")] public string Text { get; set; } = string.Empty;
    [JsonProperty("time")] public LineTime Time { get; set; } = new();

    public class LineTime
    {
        [JsonProperty("total")] public double Total { get; set; }
        [JsonProperty("minutes")] public int Minutes { get; set; }
        [JsonProperty("seconds")] public int Seconds { get; set; }
        [JsonProperty("hundredths")] public int Hundredths { get; set; }
    }
}
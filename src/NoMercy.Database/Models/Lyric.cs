using Newtonsoft.Json;

namespace NoMercy.Database.Models;
public class Lyric
{
    [JsonProperty("text")] public string Text;
    [JsonProperty("time")] public LineTime Time;

    public class LineTime
    {
        [JsonProperty("total")] public double Total;
        [JsonProperty("minutes")] public int Minutes;
        [JsonProperty("seconds")] public int Seconds;
        [JsonProperty("hundredths")] public int Hundredths;
    }
}
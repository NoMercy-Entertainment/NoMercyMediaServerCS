using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.Socket;
public class PlayerState
{
    [JsonProperty("play_State")] public string PlayState { get; set; } = "idle";
    [JsonProperty("time")] public int Time { get; set; }
    [JsonProperty("volume")] public int Volume { get; set; } = 20;
    [JsonProperty("muted")] public bool Muted { get; set; }
    [JsonProperty("shuffle")] public bool Shuffle { get; set; }
    [JsonProperty("repeat")] public string Repeat { get; set; } = "none";
    [JsonProperty("percentage")] public int Percentage { get; set; }
    [JsonProperty("current_device")] public string? CurrentDevice { get; set; }
    [JsonProperty("current_playlist")] public string? CurrentPlaylist { get; set; }
    [JsonProperty("currentItem")] public Song? CurrentItem { get; set; }
    [JsonProperty("queue")] public IEnumerable<Song> Queue { get; set; } = [];
    [JsonProperty("backlog")] public IEnumerable<Song> Backlog { get; set; } = [];
}
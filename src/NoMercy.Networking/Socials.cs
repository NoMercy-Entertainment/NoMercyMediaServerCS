using Newtonsoft.Json;

namespace NoMercy.Networking;
public class Socials
{
    [JsonProperty("twitch")] public Uri? Twitch { get; set; }
    [JsonProperty("youtube")] public Uri? Youtube { get; set; }
    [JsonProperty("twitter")] public Uri? Twitter { get; set; }
    [JsonProperty("discord")] public string Discord { get; set; } = string.Empty;
}
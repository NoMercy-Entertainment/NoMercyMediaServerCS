using Newtonsoft.Json;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.FanArt.Models;
public class MusicLabel
{
    private Uri __url;

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("url")]
    public Uri Url
    {
        get => __url.ToHttps();
        init => __url = value;
    }

    [JsonProperty("colour")] public string Color { get; set; }
    [JsonProperty("likes")] public string Likes { get; set; }
}
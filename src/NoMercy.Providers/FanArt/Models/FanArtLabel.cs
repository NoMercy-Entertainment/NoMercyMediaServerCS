#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.FanArt.Models;

public class FanArtLabel
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("musiclabel")] public MusicLabel[] Labels { get; set; }
}

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
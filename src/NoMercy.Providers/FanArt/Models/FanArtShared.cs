#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.FanArt.Models;

public class Image
{
    private Uri __url;

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("url")]
    public Uri Url
    {
        get => __url.ToHttps();
        init => __url = value;
    }

    [JsonProperty("likes")] public int Likes { get; set; }
}

public class CdArt : Image
{
    [JsonProperty("disc")] public int Disc { get; set; }
    [JsonProperty("size")] public int Size { get; set; }
}

public class VideoImage : Image
{
    [JsonProperty("lang")] public string Language { get; set; }
}
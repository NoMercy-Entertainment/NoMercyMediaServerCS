#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.CoverArt.Models;

public class CoverArtCovers
{
    [JsonProperty("images")] public CoverArtImage[] Images { get; set; }
    [JsonProperty("release")] private string Release { get; set; }
}

public class CoverArtImage
{
    private Uri? __image;

    [JsonProperty("approved")] public bool Approved { get; set; }
    [JsonProperty("back")] public bool Back { get; set; }
    [JsonProperty("comment")] public string Comment { get; set; }
    [JsonProperty("edit")] public int Edit { get; set; }
    [JsonProperty("front")] public bool Front { get; set; }
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("image")]
    public Uri? Image
    {
        get => __image?.ToHttps();
        init => __image = value;
    }

    [JsonProperty("thumbnails")] public CoverArtThumbnails CoverArtThumbnails { get; set; }
    [JsonProperty("types")] public string[] Types { get; set; }
}

public class CoverArtThumbnails
{
    private readonly Uri __250;
    private readonly Uri __500;
    private readonly Uri __1200;
    private readonly Uri __large;
    private readonly Uri __small;

    [JsonProperty("250")]
    public Uri _250
    {
        get => __250.ToHttps();
        init => __250 = value;
    }

    [JsonProperty("500")]
    public Uri _500
    {
        get => __500.ToHttps();
        init => __500 = value;
    }

    [JsonProperty("1200")]
    public Uri _1200
    {
        get => __1200.ToHttps();
        init => __1200 = value;
    }

    [JsonProperty("large")]
    public Uri Large
    {
        get => __large.ToHttps();
        init => __large = value;
    }

    [JsonProperty("small")]
    public Uri Small
    {
        get => __small.ToHttps();
        init => __small = value;
    }
}

public enum CoverArtType
{
    Back,
    BackSpine,
    Booklet,
    Bottom,
    Front,
    Liner,
    Medium,
    Obi,
    Other,
    Poster,
    Spine,
    Sticker,
    Top,
    Track,
    Tray,
    Watermark
}
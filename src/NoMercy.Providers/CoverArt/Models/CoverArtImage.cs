using Newtonsoft.Json;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.CoverArt.Models;
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
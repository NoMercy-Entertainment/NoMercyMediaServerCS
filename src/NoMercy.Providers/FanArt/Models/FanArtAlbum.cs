#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;

public class FanArtAlbum
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("mbid_id")] public string MbId { get; set; }
    [JsonProperty("albums")] public Dictionary<Guid, Albums> Albums { get; set; } = [];
}

public class Albums
{
    private Image[]? _cover = [];
    private Image[]? _cdart = [];

    [JsonProperty("albumcover")]
    public Image[] Cover
    {
        get => _cover ?? [];
        set => _cover = value;
    }

    [JsonProperty("cdart")]
    public Image[] CdArt
    {
        get => _cdart ?? [];
        set => _cdart = value;
    }
}
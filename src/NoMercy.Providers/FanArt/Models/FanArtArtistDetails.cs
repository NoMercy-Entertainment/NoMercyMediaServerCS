using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;
public class FanArtArtistDetails : FanArtArtist
{
    [JsonProperty("artistthumb")] public Image[] Thumbs { get; set; } = [];
    [JsonProperty("albums")] public Dictionary<Guid, Albums> ArtistAlbum { get; set; } = [];
    [JsonProperty("artistbackground")] public Image[] Backgrounds { get; set; } = [];
    [JsonProperty("hdmusiclogo")] public Image[] HdLogos { get; set; } = [];
    [JsonProperty("musiclogo")] public Image[] Logos { get; set; } = [];
    [JsonProperty("musicbanner")] public Image[] Banners { get; set; } = [];
}
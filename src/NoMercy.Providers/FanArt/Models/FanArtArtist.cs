#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;
public class FanArtArtist
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("mbid_id")] public string MbId { get; set; }
    [JsonProperty("albums")] public Dictionary<Guid, FanArtArtists> Artists { get; set; } = [];
}
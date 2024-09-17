#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.CoverArt.Models;

public class CoverArtCovers
{
    [JsonProperty("images")] public CoverArtImage[] Images { get; set; }
    [JsonProperty("release")] private string Release { get; set; }
}
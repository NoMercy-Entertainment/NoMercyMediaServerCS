#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzTag
{
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzRecording
{
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("genres")] public object[] Genres { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}
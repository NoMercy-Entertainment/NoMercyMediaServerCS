using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record TextTrackDto
{
    [JsonProperty("label")] public string Label { get; set; }
    [JsonProperty("file")] public string File { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("kind")] public string Kind { get; set; }
}
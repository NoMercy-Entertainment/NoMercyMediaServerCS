using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;
public class Audio
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("language")] public string? Language { get; set; } = string.Empty;
}
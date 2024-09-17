using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record SourceDto
{
    [JsonProperty("src")] public string Src { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("languages")] public string?[]? Languages { get; set; }
}
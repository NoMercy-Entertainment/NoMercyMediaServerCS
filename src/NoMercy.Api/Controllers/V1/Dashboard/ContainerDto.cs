using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class ContainerDto
{
    [JsonProperty("label")] public string Label { get; set; } = string.Empty;
    [JsonProperty("value")] public string Value { get; set; } = string.Empty;
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("default")] public bool IsDefault { get; set; }
}
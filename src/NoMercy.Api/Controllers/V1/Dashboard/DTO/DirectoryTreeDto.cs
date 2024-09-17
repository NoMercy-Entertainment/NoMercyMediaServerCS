using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;
public class DirectoryTreeDto
{
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("mode")] public int Mode { get; set; }
    [JsonProperty("size")] public int? Size { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("parent")] public string Parent { get; set; } = string.Empty;
    [JsonProperty("full_path")] public string FullPath { get; set; } = string.Empty;
}
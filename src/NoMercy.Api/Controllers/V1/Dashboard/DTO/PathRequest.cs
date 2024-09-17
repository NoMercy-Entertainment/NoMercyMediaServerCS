using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;
public class PathRequest
{
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
}
using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;
public class FileListResponseDto
{
    [JsonProperty("status")] public string Status { get; set; } = string.Empty;
    [JsonProperty("files")] public List<FileItemDto> Files { get; set; } = new();
}
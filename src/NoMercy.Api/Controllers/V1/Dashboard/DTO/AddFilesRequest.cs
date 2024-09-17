using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;
public class AddFilesRequest
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("files")] public string[] Files { get; set; } = [];
}
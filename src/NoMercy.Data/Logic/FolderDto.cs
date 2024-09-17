using Newtonsoft.Json;

namespace NoMercy.Data.Logic;
public class FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
}
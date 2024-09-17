using Newtonsoft.Json;

namespace NoMercy.Data.Repositories;
public class FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("encoder_profiles")] public Ulid[] EncoderProfiles { get; set; } = [];
}
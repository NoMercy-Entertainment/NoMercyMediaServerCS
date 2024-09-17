using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record HomeSourceDto(int Id, string MediaType) {
    [JsonProperty("id")] public int Id { get; set; } = Id;
    [JsonProperty("media_type")] public string MediaType { get; set; } = MediaType;

}
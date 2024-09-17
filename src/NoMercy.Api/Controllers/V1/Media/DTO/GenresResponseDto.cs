using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record GenresResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }
    [JsonProperty("data")] public List<GenresResponseItemDto> Data { get; set; } = [];
}
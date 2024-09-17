using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record SpecialsResponseDto2
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IEnumerable<SpecialResponseItemDto> Data { get; set; }
}
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record ContinueWatchingDto
{
    [JsonProperty("data")] public IEnumerable<ContinueWatchingItemDto> Data { get; set; }
}
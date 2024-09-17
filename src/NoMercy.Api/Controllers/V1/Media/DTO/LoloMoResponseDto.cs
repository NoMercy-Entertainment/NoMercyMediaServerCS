#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record LoloMoResponseDto<T>
{
    [JsonProperty("data")] public IEnumerable<LoloMoRowDto<T>> Data { get; set; } = [];
}
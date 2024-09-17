using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.DTO;
public record StatusResponseDto<T>
{
    [JsonProperty("status")] public string Status { get; set; } = "ok";
    [JsonProperty("data")] public T Data { get; set; } = default!;
    [JsonProperty("message")] public string? Message { get; set; } = "NoMercy is running!";
    [JsonProperty("args")] public dynamic[]? Args { get; set; } = [];
}
using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record Meta
{
    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("logo")] public Logo? Logo { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record ProgressDto
{
    [JsonProperty("percentage")] public int? Percentage { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
}
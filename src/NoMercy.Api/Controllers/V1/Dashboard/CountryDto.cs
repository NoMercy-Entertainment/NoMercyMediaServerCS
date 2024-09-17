using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class CountryDto
{
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("code")] public string? Code { get; set; }
}
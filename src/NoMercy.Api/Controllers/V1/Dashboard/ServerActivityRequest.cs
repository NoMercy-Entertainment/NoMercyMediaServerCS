using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class ServerActivityRequest
{
    [JsonProperty("take")] public int? Take { get; set; } = 10;
}
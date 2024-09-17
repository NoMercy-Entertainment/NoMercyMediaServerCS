using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class AniDbCredentialsResponse
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("api_key")] public string? ApiKey { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class AniDbCredentialsRequest
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("password")] public string? Password { get; set; }
    [JsonProperty("api_key")] public string ApiKey { get; set; }
}
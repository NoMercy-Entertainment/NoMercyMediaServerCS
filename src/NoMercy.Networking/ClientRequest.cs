using Newtonsoft.Json;

namespace NoMercy.Networking;
public class ClientRequest
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("custom_name")] public string CustomName { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
}
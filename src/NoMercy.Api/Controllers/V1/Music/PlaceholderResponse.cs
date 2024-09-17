using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Music;
public class PlaceholderResponse
{
    [JsonProperty("data")] public string[] Data { get; set; }
}
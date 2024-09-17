using Newtonsoft.Json;

namespace NoMercy.Networking;
public class RefreshLibraryDto
{
    [JsonProperty("queryKey")] public dynamic?[] QueryKey { get; set; } = [];
}
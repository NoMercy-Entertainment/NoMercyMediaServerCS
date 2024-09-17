using Newtonsoft.Json;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Helper;
public class ServerRegisterResponse
{
    [JsonProperty("status")] public string Status { get; set; } = string.Empty;
    [JsonProperty("id")] public string ServerId { get; set; } = string.Empty;
    [JsonProperty("user")] public User User { get; set; } = new();
}
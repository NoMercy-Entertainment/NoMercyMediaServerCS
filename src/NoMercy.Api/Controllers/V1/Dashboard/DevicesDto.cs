using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class DevicesDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string? Device { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("custom_name")] public object? CustomName { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("activity_logs")] public IEnumerable<ActivityLogDto> ActivityLogs { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class ServerActivityDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("user_id")] public Guid UserId { get; set; }
    [JsonProperty("device_id")] public Ulid DeviceId { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("user")] public string User { get; set; }
}
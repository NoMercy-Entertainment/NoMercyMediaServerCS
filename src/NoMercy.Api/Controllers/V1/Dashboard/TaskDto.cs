using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Dashboard;
public class TaskDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("value")] public int Value { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
}
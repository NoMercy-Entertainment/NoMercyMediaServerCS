using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.Socket;
public class Track
{
    [JsonProperty("color_palette")] public ColorPalette ColorPalette { get; set; } = new();
    [JsonProperty("cover")] public string Cover { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("origin")] public string Origin { get; set; } = "local";
}
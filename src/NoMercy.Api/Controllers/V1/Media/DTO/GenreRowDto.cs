using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record GenreRowDto<T>
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("moreLink")] public string? MoreLink { get; set; }
    [JsonProperty("items")] public IEnumerable<T?> Items { get; set; } = [];

    [NotMapped]
    [System.Text.Json.Serialization.JsonIgnore]
    [JsonProperty("source")]
    public IEnumerable<HomeSourceDto> Source { get; set; }
}
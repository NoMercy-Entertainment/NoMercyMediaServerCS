using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record LoloMoRowDto<T>
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("moreLink")] public string MoreLink { get; set; }
    [JsonProperty("items")] public IEnumerable<T> Items { get; set; }
}
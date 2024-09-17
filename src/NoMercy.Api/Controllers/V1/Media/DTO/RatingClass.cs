using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record RatingClass
{
    [JsonProperty("rating")] public string Rating { get; set; }
    [JsonProperty("meaning")] public string Meaning { get; set; }
    [JsonProperty("order")] public long Order { get; set; }
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("image")] public string Image { get; set; }
}
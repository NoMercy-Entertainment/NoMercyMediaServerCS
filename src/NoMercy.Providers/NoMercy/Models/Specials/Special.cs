using Newtonsoft.Json;

namespace NoMercy.Providers.NoMercy.Models.Specials;

public class Special
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("title")] public string Title { get; set; } = null!;
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("creator")] public string? Creator { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
}

public class CollectionItem
{
    public int index { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public int year { get; set; }
    public int[] seasons { get; set; }
    public int[] episodes { get; set; }
}
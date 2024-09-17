using Newtonsoft.Json;

namespace NoMercy.Data.Repositories;
public class LibrarySortRequestItem
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("order")] public int Order { get; set; }
}
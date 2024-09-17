using Newtonsoft.Json;

namespace NoMercy.Data.Repositories;
public class LibrarySortRequest
{
    [JsonProperty("libraries")] public LibrarySortRequestItem[] Libraries { get; set; } = [];
}
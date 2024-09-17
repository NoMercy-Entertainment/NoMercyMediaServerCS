using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.People;

public class TmdbPersonTaggedImages : TmdbPaginatedResponse<TmdbTaggedImage>
{
    [JsonProperty("id")] public string Id { get; set; }
}
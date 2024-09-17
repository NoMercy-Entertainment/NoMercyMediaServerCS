using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Collections;

public class TmdbCollectionDetails : TmdbCollection
{
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("parts")] public TmdbMovie[] Parts { get; set; }
}
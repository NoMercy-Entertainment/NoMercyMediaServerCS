using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbLogo
{
    [JsonProperty("aspect_ratio")] public double AspectRatio { get; set; }
    [JsonProperty("file_path")] public string FilePath { get; set; }
    [JsonProperty("height")] public int Height { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("file_type")] public string FileType { get; set; }
    [JsonProperty("vote_average")] public int VoteAverage { get; set; }
    [JsonProperty("vote_count")] public int VoteCount { get; set; }
    [JsonProperty("width")] public int Width { get; set; }
}
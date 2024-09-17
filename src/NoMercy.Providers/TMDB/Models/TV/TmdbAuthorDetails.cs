using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;
public class TmdbAuthorDetails
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("avatar_path")] public string AvatarPath { get; set; }
    [JsonProperty("rating")] public int Rating { get; set; }
}
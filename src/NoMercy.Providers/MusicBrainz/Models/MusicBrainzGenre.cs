#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;

public class MusicBrainzAllGenres
{
    [JsonProperty("genres")] public MusicBrainzGenre[] Genres { get; set; }
    [JsonProperty("genre-offset")] public long GenreOffset { get; set; }
    [JsonProperty("genre-count")] public long GenreCount { get; set; }
}

public class MusicBrainzGenre
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}

public class MusicBrainzGenreDetails : MusicBrainzGenre
{
    [JsonProperty("count")] public long Count { get; set; }
}
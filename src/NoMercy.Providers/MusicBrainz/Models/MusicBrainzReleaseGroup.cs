using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzReleaseGroup
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }

    [JsonProperty("first-release-date")] private string? _firstReleaseDate { get; set; }

    public DateTime? FirstReleaseDate
    {
        get => !string.IsNullOrWhiteSpace(_firstReleaseDate) && !string.IsNullOrEmpty(_firstReleaseDate) && _firstReleaseDate.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _firstReleaseDate = value.ToString() ?? string.Empty;
    }

    [JsonProperty("genres")] public MusicBrainzGenreDetails[]? Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("primary-type")] public string PrimaryType { get; set; }
    [JsonProperty("primary-type-id")] public Guid? PrimaryTypeId { get; set; }
    [JsonProperty("secondary-type-ids")] public Guid[] SecondaryTypeIds { get; set; }
    [JsonProperty("secondary-types")] public string[] SecondaryTypes { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Filename), IsUnique = true)]
[Index(nameof(EpisodeId))]
[Index(nameof(MovieId))]
public class VideoFile : Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public Ulid Id { get; set; } = Ulid.NewUlid();

    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("filename")] public string Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("host_folder")] public string HostFolder { get; set; }
    [JsonProperty("languages")] public string Languages { get; set; }
    [JsonProperty("quality")] public string Quality { get; set; }
    [JsonProperty("share")] public string Share { get; set; }
    [JsonProperty("subtitles")] public string? Subtitles { get; set; }
    [JsonProperty("chapters")] public string? Chapters { get; set; }

    [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
    public Episode Episode { get; set; }

    [JsonProperty("movie_id")] public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    [JsonProperty("user_data")] public ICollection<UserData> UserData { get; set; }
}
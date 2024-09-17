#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Playlist : ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public required Ulid Id { get; set; } = Ulid.NewUlid();

    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }

    [JsonProperty("user_id")] public required Guid UserId { get; set; }
    public User User { get; set; }

    [JsonProperty("playlist_track")] public ICollection<PlaylistTrack> Tracks { get; set; }
}
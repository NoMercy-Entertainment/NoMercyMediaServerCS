#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Album : ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("country")] public string? Country { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("tracks")] public int Tracks { get; set; }

    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("host_folder")] public string? HostFolder { get; set; }

    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    public Library Library { get; set; }

    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    public Folder LibraryFolder { get; set; }

    [JsonProperty("album_track")] public ICollection<AlbumTrack> AlbumTrack { get; set; }
    [JsonProperty("album_artist")] public ICollection<AlbumArtist> AlbumArtist { get; set; }
    [JsonProperty("album_user")] public ICollection<AlbumUser> AlbumUser { get; set; }
    [JsonProperty("album_genre")] public ICollection<AlbumMusicGenre> AlbumMusicGenre { get; set; }
    [JsonProperty("album_release")] public ICollection<AlbumReleaseGroup> AlbumReleaseGroup { get; set; }
    [JsonProperty("translations")] public ICollection<Translation> Translations { get; set; }
    [JsonProperty("images")] public ICollection<Image> Images { get; set; }
}
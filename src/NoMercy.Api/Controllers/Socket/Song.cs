using Newtonsoft.Json;

namespace NoMercy.Api.Controllers.Socket;
public class Song
{
    [JsonProperty("color_palette")] public ColorPalette ColorPalette { get; set; } = new();
    [JsonProperty("cover")] public string Cover { get; set; } = string.Empty;
    [JsonProperty("date")] public DateTime Date { get; set; }
    [JsonProperty("disc")] public int Disc { get; set; }
    [JsonProperty("duration")] public string Duration { get; set; } = string.Empty;
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string Filename { get; set; } = string.Empty;
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("origin")] public string Origin { get; set; } = "local";
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("quality")] public int Quality { get; set; }
    [JsonProperty("track")] public int Track { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = "audio";

    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }
    [JsonProperty("artist_track")] public Track[] ArtistTrack { get; set; } = [];
    [JsonProperty("album_track")] public Track[] AlbumTrack { get; set; } = [];
    [JsonProperty("full_cover")] public string FullCover { get; set; } = string.Empty;
}
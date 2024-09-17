using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record AlbumTrackDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
    [JsonProperty("disc")] public int? Disc { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("quality")] public int? Quality { get; set; }
    [JsonProperty("track")] public int? Track { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }

    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }
    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }

    public AlbumTrackDto(AlbumTrack albumTrack, string country)
    {
        ColorPalette = albumTrack.Album.ColorPalette;
        Cover = albumTrack.Album.Cover;
        Date = albumTrack.Track.Date;
        Disc = albumTrack.Track.DiscNumber;
        Duration = albumTrack.Track.Duration;
        Favorite = albumTrack.Track.TrackUser.Count != 0;
        Filename = albumTrack.Track.Filename;
        Folder = albumTrack.Track.Folder;
        FolderId = albumTrack.Track.FolderId;
        Id = albumTrack.Track.Id;
        LibraryId = albumTrack.Album.LibraryId;
        Name = albumTrack.Track.Name;
        Origin = NmSystem.Info.DeviceId;
        Path = albumTrack.Track.Folder + "/" + albumTrack.Track.Filename;
        Quality = albumTrack.Track.Quality;
        Track = albumTrack.Track.TrackNumber;
        Lyrics = albumTrack.Track.Lyrics;
        Type = "tracks";

        using MediaContext mediaContext = new();
        List<ArtistTrack> artists = mediaContext.ArtistTrack
            .Where(at => at.TrackId == albumTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();

        Artist = artists
            // .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(artistTrack => new ArtistDto(artistTrack, country));

        Album = albumTrack.Track.AlbumTrack
            // .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(trackAlbum => new AlbumDto(trackAlbum, country));
    }
}
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record PlaylistTrackDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
    [JsonProperty("disc")] public int? Disc { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("quality")] public int? Quality { get; set; }
    [JsonProperty("track")] public int? Track { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }

    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }
    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }

    public PlaylistTrackDto(ArtistTrack artistTrack, string country)
    {
        ColorPalette = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.ColorPalette;
        Cover = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.Cover;
        Date = artistTrack.Track.Date;
        Disc = artistTrack.Track.DiscNumber;
        Duration = artistTrack.Track.Duration;
        Favorite = artistTrack.Track.TrackUser.Any();
        Filename = artistTrack.Track.Filename;
        Folder = artistTrack.Track.Folder;
        FolderId = artistTrack.Track.FolderId;
        Id = artistTrack.Track.Id;
        LibraryId = artistTrack.Artist.LibraryId;
        Name = artistTrack.Track.Name;
        Origin = NmSystem.Info.DeviceId;
        Path = artistTrack.Track.Folder + "/" + artistTrack.Track.Filename;
        Quality = artistTrack.Track.Quality;
        Track = artistTrack.Track.TrackNumber;
        Lyrics = artistTrack.Track.Lyrics;
        Type = "tracks";

        Album = artistTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country));

        using MediaContext mediaContext = new();
        List<ArtistTrack> artists = mediaContext.ArtistTrack
            .Where(at => at.TrackId == artistTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();

        Artist = artists
            .Select(albumTrack => new ArtistDto(albumTrack, country));
    }

    public PlaylistTrackDto(PlaylistTrack trackTrack, string country)
    {
        ColorPalette = trackTrack.Track.AlbumTrack.FirstOrDefault()?.Album.ColorPalette;
        Cover = trackTrack.Track.AlbumTrack.FirstOrDefault()?.Album.Cover;
        Date = trackTrack.Track.Date;
        Disc = trackTrack.Track.DiscNumber;
        Duration = trackTrack.Track.Duration;
        Favorite = trackTrack.Track.TrackUser.Any();
        Filename = trackTrack.Track.Filename;
        Folder = trackTrack.Track.Folder;
        FolderId = trackTrack.Track.FolderId;
        Id = trackTrack.Track.Id;
        LibraryId = trackTrack.Track.AlbumTrack.FirstOrDefault()?.Album.LibraryId;
        Name = trackTrack.Track.Name;
        Origin = NmSystem.Info.DeviceId;
        Path = trackTrack.Track.Folder + "/" + trackTrack.Track.Filename;
        Quality = trackTrack.Track.Quality;
        Track = trackTrack.Track.TrackNumber;
        Lyrics = trackTrack.Track.Lyrics;
        Type = "tracks";

        Album = trackTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country));

        using MediaContext mediaContext = new();
        List<ArtistTrack> artists = mediaContext.ArtistTrack
            .Where(at => at.TrackId == trackTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();

        Artist = artists
            .Select(albumTrack => new ArtistDto(albumTrack, country));
    }
}
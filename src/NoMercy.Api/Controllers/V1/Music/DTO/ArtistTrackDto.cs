using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record ArtistTrackDto
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
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    [JsonProperty("album_name")] public string AlbumName { get; set; }

    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }
    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }

    public ArtistTrackDto(ArtistTrack artistTrack, string country)
    {
        ColorPalette = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.ColorPalette;
        Cover = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.Cover;
        Date = artistTrack.Track.Date;
        Disc = artistTrack.Track.DiscNumber;
        AlbumId = artistTrack.Track.AlbumTrack.First().AlbumId;
        AlbumName = artistTrack.Track.AlbumTrack.First().Album.Name;
        Duration = artistTrack.Track.Duration;
        Favorite = artistTrack.Track.TrackUser.Any();
        Filename = artistTrack.Track.Filename;
        Folder = artistTrack.Track.Folder;
        FolderId = artistTrack.Track.FolderId;
        Id = artistTrack.Track.Id;
        LibraryId = artistTrack.Artist.LibraryId;
        Name = artistTrack.Track.Name;
        Origin = Info.DeviceId;
        Path = artistTrack.Track.Folder + "/" + artistTrack.Track.Filename;
        Quality = artistTrack.Track.Quality;
        Track = artistTrack.Track.TrackNumber;
        Lyrics = artistTrack.Track.Lyrics;
        Type = "tracks";

        Album = artistTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country));

        Artist = artistTrack.Track.ArtistTrack
            .Select(albumTrack => new ArtistDto(albumTrack, country));
    }


    public ArtistTrackDto(Track track, string? country = "US")
    {
        ColorPalette = track.AlbumTrack.First().Album.ColorPalette ?? track.ArtistTrack.First().Artist.ColorPalette;
        Cover = track.AlbumTrack.First().Album.Cover ?? track.ArtistTrack.First().Artist.Cover;
        Date = track.UpdatedAt;
        Disc = track.DiscNumber;
        Duration = track.Duration;
        Favorite = track.TrackUser.Any();
        Filename = track.Filename;
        Folder = track.Folder;
        FolderId = track.FolderId;
        Id = track.Id;
        LibraryId = track.AlbumTrack.First().Album.LibraryId;
        Name = track.Name;
        Origin = Info.DeviceId;
        Path = track.Folder + "/" + track.Filename;
        Quality = track.Quality;
        Track = track.TrackNumber;
        Type = "tracks";
        AlbumName = track.AlbumTrack.First().Album.Name;

        Album = track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country!));

        Artist = track.ArtistTrack
            .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(trackArtist => new ArtistDto(trackArtist, country!));
    }
}
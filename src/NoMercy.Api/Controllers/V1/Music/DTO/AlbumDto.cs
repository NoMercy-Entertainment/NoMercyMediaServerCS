using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record AlbumDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("tracks")] public int Tracks { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("album_artist")] public Guid? AlbumArtist { get; set; }

    public AlbumDto(AlbumArtist albumArtist, string country)
    {
        string? description = albumArtist.Album.Translations?
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumArtist.Album.Description;

        ColorPalette = albumArtist.Album.ColorPalette;
        Cover = albumArtist.Album.Cover;
        Disambiguation = albumArtist.Album.Disambiguation;
        Folder = albumArtist.Album.Folder;
        Id = albumArtist.Album.Id;
        LibraryId = albumArtist.Album.LibraryId;
        Name = albumArtist.Album.Name;
        Origin = NmSystem.Info.DeviceId;
        Type = "albums";
        Tracks = albumArtist.Album.AlbumTrack?.Count ?? 0;
        Year = albumArtist.Album.Year;

        AlbumArtist = albumArtist.ArtistId;
    }

    public AlbumDto(AlbumTrack albumTrack, string country)
    {
        string? description = albumTrack.Album.Translations?
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumTrack.Album.Description;

        ColorPalette = albumTrack.Album.ColorPalette;
        Cover = albumTrack.Album.Cover;
        Disambiguation = albumTrack.Album.Disambiguation;
        Folder = albumTrack.Album.Folder;
        Id = albumTrack.Album.Id;
        LibraryId = albumTrack.Album.LibraryId;
        Name = albumTrack.Album.Name;
        // Tracks = albumTrack.Album.AlbumTrack.Count(at => at.Track.Folder != null);
        Year = albumTrack.Album.Year;
        Origin = NmSystem.Info.DeviceId;
        Type = "albums";

        using MediaContext mediaContext = new();
        int? tracks = mediaContext.Albums
            .Include(a => a.AlbumTrack)
            .ThenInclude(at => at.Track)
            .FirstOrDefault(a => a.Id == albumTrack.AlbumId)?.AlbumTrack
            .Count(at => at.Track.Folder != null);
        Tracks = tracks ?? 0;

        AlbumArtist = albumTrack.Album.AlbumArtist?.MaxBy(at => at.ArtistId)?.ArtistId;
    }

    public AlbumDto(Album album, string country)
    {
        string? description = album.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : album.Description;

        ColorPalette = album.ColorPalette;
        Cover = album.Cover;
        Disambiguation = album.Disambiguation;
        Description = album.Description;
        Folder = album.Folder;
        Id = album.Id;
        LibraryId = album.LibraryId;
        Name = album.Name;
        Tracks = album.AlbumTrack.Count(at => at.Track.Folder != null);
        Year = album.Year;
        Origin = NmSystem.Info.DeviceId;
        Type = "albums";

        List<IGrouping<Guid, AlbumArtist>> artists = album.AlbumArtist
            .GroupBy(albumArtist => albumArtist.ArtistId)
            .OrderBy(artist => artist.Count())
            .ToList();

        int trackCount = album.Tracks;

        int? artistTrackCount = album.AlbumTrack?
            .Select(albumTrack => albumTrack.Track)
            .SelectMany(track => track.ArtistTrack)
            .Count();
        //
        // var realTrackCount = album.AlbumTrack?
        //     .Select(albumTrack => albumTrack.Track)
        //     .SelectMany(track => track.ArtistTrack)
        //     .Count(at => at.Track.Folder != null);
        //
        // if (artistTrackCount == 1 || artistTrackCount != realTrackCount)
        // {

        using MediaContext mediaContext = new();
        int? tracks = mediaContext.Albums
            .Include(a => a.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Track)
            .FirstOrDefault(a => a.Id == album.Id)?.AlbumTrack
            .Count(at => at.Track.Folder != null);

        Tracks = tracks ?? 0;
        // }

        bool isAlbumArtist = artistTrackCount >= trackCount * 0.45;

        AlbumArtist = isAlbumArtist
            ? artists.FirstOrDefault()?.Key
            : null;
    }
}
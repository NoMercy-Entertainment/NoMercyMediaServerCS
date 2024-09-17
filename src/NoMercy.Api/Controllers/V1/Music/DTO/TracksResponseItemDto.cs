using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record TracksResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("country")] public string? Country { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }

    [JsonProperty("artists")] public List<ArtistDto> Artists { get; set; }
    [JsonProperty("albums")] public List<AlbumDto> Albums { get; set; }
    [JsonProperty("tracks")] public List<ArtistTrackDto> Tracks { get; set; }

    public TracksResponseItemDto()
    {
    }

    public TracksResponseItemDto(Track track, string country)
    {
        ColorPalette = track.ColorPalette;
        Cover = track.Cover;
        Favorite = track.TrackUser.Any();
        Folder = track.Folder;
        Id = track.Id;
        LibraryId = track.AlbumTrack.First().Album.LibraryId;
        Name = track.Name;
        Type = "favorites";

        Artists = track.ArtistTrack
            .Select(trackArtist => new ArtistDto(trackArtist, country))
            .ToList();

        Albums = track.AlbumTrack
            .Select(albumTrack => new AlbumDto(albumTrack, country))
            .ToList();

        Tracks = track.ArtistTrack
            .Select(albumTrack => new ArtistTrackDto(albumTrack, country))
            .ToList();
    }
}
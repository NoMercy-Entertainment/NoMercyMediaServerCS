using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Music.DTO;
public record ArtistDto
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("type")] public string Type { get; set; }

    public ArtistDto(AlbumArtist albumArtist, string country)
    {
        string? description = albumArtist.Artist.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumArtist.Album.Description;

        Id = albumArtist.Artist.Id;
        Name = albumArtist.Artist.Name;
        Disambiguation = albumArtist.Artist.Disambiguation;
        Cover = albumArtist.Artist.Cover;
        Folder = albumArtist.Artist.Folder;
        ColorPalette = albumArtist.Artist.ColorPalette;
        LibraryId = albumArtist.Artist.LibraryId;
        Origin = Info.DeviceId;
        Type = "artists";
    }

    public ArtistDto(ArtistTrack artistTrack, string country)
    {
        // string? description = artistTrack.Artist.Translations
        //     .FirstOrDefault(translation => translation.Iso31661 == country)?
        //     .Description;

        // Description = !string.IsNullOrEmpty(description)
        //     ? description
        //     : artistTrack.Artist.Description;
        Description = artistTrack.Artist.Description;

        Id = artistTrack.Artist.Id;
        Name = artistTrack.Artist.Name;
        Disambiguation = artistTrack.Artist.Disambiguation;
        Cover = artistTrack.Artist.Cover;
        Folder = artistTrack.Artist.Folder;
        ColorPalette = artistTrack.Artist.ColorPalette;
        LibraryId = artistTrack.Artist.LibraryId;
        Origin = Info.DeviceId;
        Type = "artists";
    }
}
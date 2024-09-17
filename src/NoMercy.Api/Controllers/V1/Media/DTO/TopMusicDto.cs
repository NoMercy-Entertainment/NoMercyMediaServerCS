using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record TopMusicDto
{
    [JsonProperty("id")] public string Id { get; set; } = "";
    [JsonProperty("name")] public string Name { get; set; } = "";
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = "albums";
    [JsonProperty("cover")] public string? Cover { get; set; }

    public TopMusicDto()
    {
    }

    public TopMusicDto(PlaylistTrack musicPlay)
    {
        Id = musicPlay.Playlist.Id.ToString();
        Name = musicPlay.Playlist.Name;
        ColorPalette = musicPlay.Playlist.ColorPalette;
        Type = "playlists";
        Cover = musicPlay.Playlist.Cover;
    }

    public TopMusicDto(AlbumTrack albumTrack)
    {
        Id = albumTrack.Album.Id.ToString();
        Name = albumTrack.Album.Name;
        ColorPalette = albumTrack.Album.ColorPalette;
        Type = "albums";
        Cover = albumTrack.Album.Cover;
    }

    public TopMusicDto(ArtistTrack artistTrack)
    {
        Id = artistTrack.Artist.Id.ToString();
        Name = artistTrack.Artist.Name;
        ColorPalette = artistTrack.Artist.ColorPalette;
        Type = "artists";
        Cover = artistTrack.Artist.Cover;
    }
}
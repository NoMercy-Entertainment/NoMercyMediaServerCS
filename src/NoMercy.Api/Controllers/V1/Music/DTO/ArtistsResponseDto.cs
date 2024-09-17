using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;

public record ArtistsResponseDto
{
    [JsonProperty("data")] public IEnumerable<ArtistsResponseItemDto> Data { get; set; } = [];

    private static readonly string[] Letters = ["*", "#", "'", "\"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];

    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<Artist>> GetArtists =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string letter) =>
            mediaContext.Artists
                .AsNoTracking()
                .Where(album => letter == "_"
                    ? Letters.Any(p => album.Name.StartsWith(p))
                    : album.Name.StartsWith(letter)
                )
                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .Where(artist => artist.ArtistTrack.Any(artistTrack => artistTrack.Track.Duration != null))
                .GroupBy(artist => artist.Name).Select(x => x.First())
        );
}

public record ArtistsResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("track_id")] public string? TrackId { get; set; }
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("tracks")] public int Tracks { get; set; }

    public ArtistsResponseItemDto(Artist artist)
    {
        ColorPalette = artist.ColorPalette;
        Cover = artist.Cover;
        Disambiguation = artist.Disambiguation;
        Description = artist.Description;
        Id = artist.Id;
        Name = artist.Name;
        Type = "artists";

        Tracks = artist.ArtistTrack
            .Select(artistTrack => artistTrack.Track)
            .Count(artistTrack => artistTrack.Duration != null);
    }
}
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record ScreensaverDto
{
    [JsonProperty("data")] public IEnumerable<ScreensaverDataDto> Data { get; set; }
}

public record ScreensaverDataDto
{
    [JsonProperty("aspectRatio")] public double AspectRatio { get; set; }

    [JsonProperty("src")] public string? Src { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("meta")] public Meta? Meta { get; set; }

    public ScreensaverDataDto(Image image, IEnumerable<Image> logos, string type)
    {
        Image? logo = logos.FirstOrDefault(x =>
            (type == "tv" && x.TvId == image.TvId)
            || (type == "movie" && x.MovieId == image.MovieId));

        string name = type == "tv"
            ? image.Tv.Title
            : image.Movie.Title;

        AspectRatio = image.AspectRatio;
        Src = image.FilePath;
        ColorPalette = image.ColorPalette;
        Meta = new Meta
        {
            Title = name,
            Logo = logo != null
                ? new Logo
                {
                    AspectRatio = logo.AspectRatio,
                    Src = logo.FilePath
                }
                : null
        };
    }
}

public record Meta
{
    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("logo")] public Logo? Logo { get; set; }
}

public record Logo
{
    [JsonProperty("aspectRatio")] public double AspectRatio { get; set; }

    [JsonProperty("src")] public string? Src { get; set; }
}
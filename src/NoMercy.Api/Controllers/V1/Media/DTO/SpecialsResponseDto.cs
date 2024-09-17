#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record SpecialsResponseDto2
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IEnumerable<SpecialResponseItemDto> Data { get; set; }
}

public record SpecialsResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<SpecialsResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<SpecialsResponseItemDto>> GetSpecials =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Specials.AsNoTracking()
                .Include(special => special.Items)
                .ThenInclude(item => item.Episode)
                .ThenInclude(episode => episode!.Tv)
                .Include(special => special.Items)
                .ThenInclude(item => item.Episode)
                .ThenInclude(episode => episode!.VideoFiles)
                .Include(special => special.Items)
                .ThenInclude(item => item.Movie)
                .ThenInclude(movie => movie!.VideoFiles)
                .Select(special => new SpecialsResponseItemDto(special))
        );
}

public record SpecialsResponseItemDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }
    [JsonProperty("videoId")] public string? VideoId { get; set; }
    [JsonProperty("videos")] public VideoDto[]? Videos { get; set; } = [];

    [JsonProperty("total_duration")] public int TotalDuration { get; set; }

    public SpecialsResponseItemDto(SpecialItem item)
    {
        if (item.Movie is null) return;

        string? title = item.Movie.Translations.FirstOrDefault()?.Title;
        string? overview = item.Movie.Translations.FirstOrDefault()?.Overview;

        Id = item.Movie.Id.ToString();
        Title = !string.IsNullOrEmpty(title)
            ? title
            : item.Movie.Title;
        Overview = !string.IsNullOrEmpty(overview)
            ? overview
            : item.Movie.Overview;

        Backdrop = item.Movie.Backdrop;
        Logo = item.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "item";
        Year = item.Movie.ReleaseDate.ParseYear();
        ColorPalette = item.Movie.ColorPalette;
        Poster = item.Movie.Poster;
        TitleSort = item.Movie.Title.TitleSort(item.Movie.ReleaseDate);
        Type = "item";
        Genres = item.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = item.Movie.Video;
        Videos = item.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public SpecialsResponseItemDto(Special special)
    {
        Id = special.Id.ToString();
        Title = special.Title;
        Overview = special.Description;
        Backdrop = special.Backdrop;
        // Logo = special.Images
        //     .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        MediaType = "specials";
        Year = special.CreatedAt.ParseYear();

        ColorPalette = special.ColorPalette;
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();

        Type = "specials";

        NumberOfItems = special.Items.Count;

        int haveMovies = special.Items
            .Select(item => item.Movie)
            .Count(movie => movie is not null && movie.VideoFiles.Any());

        int haveEpisodes = special.Items
            .Select(item => item.Episode)
            .Count(movie => movie is not null && movie.VideoFiles.Any());

        HaveItems = haveMovies + haveEpisodes;

        int[] movies = special.Items
            .Where(item => item.MovieId is not null)
            .Select(item => item.Movie?.VideoFiles.FirstOrDefault()?.Duration?.ToSeconds() ?? 0)
            .ToArray();

        int[] episodes = special.Items
            .Where(item => item.EpisodeId is not null)
            .Select(item => item.Episode?.VideoFiles.FirstOrDefault()?.Duration?.ToSeconds() ?? 0)
            .ToArray();

        TotalDuration = movies.Sum() + episodes.Sum();


        // VideoId = special.SpecialMovies?
        //     .FirstOrDefault()
        //     ?.Movie.Video;
    }
}
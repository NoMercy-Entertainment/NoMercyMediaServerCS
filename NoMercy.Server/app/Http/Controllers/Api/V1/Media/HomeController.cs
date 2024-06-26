using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}")]
public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        List<GenreRowDto<GenreRowItemDto>> genres = [];

        List<int> movieIds = [];
        List<int> tvIds = [];

        await foreach (var genre in HomeResponseDto.GetHome(mediaContext, userId,
                           HttpContext.Request.Headers.AcceptLanguage.LastOrDefault()))
        {
            GenreRowDto<GenreRowItemDto> genreRowDto = new()
            {
                Title = genre.Name,
                MoreLink = $"/genre/{genre.Id}",
                Id = genre.Id.ToString(),

                Source = genre.GenreMovies.Select(movie => new HomeSourceDto(movie.MovieId, "movie"))
                    .Concat(genre.GenreTvShows.Select(tv => new HomeSourceDto(tv.TvId, "tv")))
                    .Randomize()
                    .Take(36)
            };

            tvIds.AddRange(genreRowDto.Source
                .Where(source => source?.MediaType == "tv")
                .Select(source => source!.Id));

            movieIds.AddRange(genreRowDto.Source
                .Where(source => source?.MediaType == "movie")
                .Select(source => source!.Id));

            genres.Add(genreRowDto);
        }

        List<Tv> tvData = [];
        await foreach (var tv in HomeResponseDto.GetHomeTvs(mediaContext, tvIds,
                           HttpContext.Request.Headers.AcceptLanguage.LastOrDefault())) tvData.Add(tv);

        List<Movie> movieData = [];
        await foreach (var movie in HomeResponseDto.GetHomeMovies(mediaContext, movieIds,
                           HttpContext.Request.Headers.AcceptLanguage.LastOrDefault())) movieData.Add(movie);

        foreach (var genre in genres)
            genre.Items = genre.Source
                .Select(source =>
                {
                    switch (source?.MediaType)
                    {
                        case "tv":
                        {
                            var tv = tvData.FirstOrDefault(tv => tv.Id == source.Id);
                            return tv?.Id == null ? null : new GenreRowItemDto(tv);
                        }
                        case "movie":
                        {
                            var movie = movieData.FirstOrDefault(movie => movie.Id == source.Id);
                            return movie?.Id == null ? null : new GenreRowItemDto(movie);
                        }
                        default:
                        {
                            return null;
                        }
                    }
                })
                .Where(genreRow => genreRow != null)
                .ToList()!;

        HomeResponseDto<GenreRowItemDto> result = new()
        {
            Data = genres
        };

        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("/status")]
    public StatusResponseDto<string> Status()
    {
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "NoMercy is running!",
            Data = "v1"
        };
    }

    [HttpGet]
    [Route("screensaver")]
    public async Task<ScreensaverDto> Screensaver()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var data = await mediaContext.Images
            .AsNoTracking()
            .Where(image => image.Movie.Library.LibraryUsers
                                .FirstOrDefault(u => u.UserId == userId) != null ||
                            image.Tv.Library.LibraryUsers
                                .FirstOrDefault(u => u.UserId == userId) != null
            )
            .Where(image => image.Height > 1080)
            .Where(image => image._colorPalette != "")
            .Where(image =>
                (image.Type == "backdrop" && image.VoteAverage > 2 && image.Iso6391 == null) ||
                (image.Type == "logo" && image.Iso6391 == "en")
            )
            .Include(image => image.Movie)
            .Include(image => image.Tv)
            .ToListAsync();

        var tvCollection = data.Where(image => image is { Type: "backdrop", Tv: not null })
            .DistinctBy(image => image.TvId);
        var movieCollection = data.Where(image => image is { Type: "backdrop", Movie: not null })
            .DistinctBy(image => image.MovieId);
        var logos = data.Where(image => image is { Type: "logo" });

        return new ScreensaverDto
        {
            Data = tvCollection
                .Select(image => new ScreensaverDataDto(image, logos, "tv"))
                .Concat(movieCollection.Select(image => new ScreensaverDataDto(image, logos, "movie")))
                .Where(image => image.Meta?.Logo != null)
                .Randomize()
        };
    }

    [HttpGet]
    [Route("/api/v{Version:apiVersion}/dashboard/permissions")]
    public async Task<PermissionsResponseDto> UserPermissions()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var user = await mediaContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();

        return new PermissionsResponseDto
        {
            Edit = user?.Owner ?? user?.Manage ?? false
        };
    }
}
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.V1.Media;

[ApiController]
[Tags("Media")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}")]
public class HomeController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PageRequestDto request)
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view home");

        await using MediaContext mediaContext = new();
        List<GenreRowDto<GenreRowItemDto>> genres = [];

        List<int> movieIds = [];
        List<int> tvIds = [];

        string language = Language();

        List<Genre> genreItems = await HomeResponseDto.GetHome(mediaContext, userId,
            language, request.Take, request.Page);

        foreach (Genre genre in genreItems)
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
        await foreach (Tv tv in HomeResponseDto.GetHomeTvs(mediaContext, tvIds, language)) tvData.Add(tv);

        List<Movie> movieData = [];
        await foreach (Movie movie in HomeResponseDto.GetHomeMovies(mediaContext, movieIds, language))
            movieData.Add(movie);

        foreach (GenreRowDto<GenreRowItemDto> genre in genres)
            genre.Items = genre.Source
                .Select(source =>
                {
                    switch (source?.MediaType)
                    {
                        case "tv":
                        {
                            Tv? tv = tvData.FirstOrDefault(tv => tv.Id == source.Id);
                            return tv?.Id == null
                                ? null
                                : new GenreRowItemDto(tv,
                                    language);
                        }
                        case "movie":
                        {
                            Movie? movie = movieData.FirstOrDefault(movie => movie.Id == source.Id);
                            return movie?.Id == null
                                ? null
                                : new GenreRowItemDto(movie,
                                    language);
                        }
                        default:
                        {
                            return null;
                        }
                    }
                })
                .Where(genreRow => genreRow != null)
                .ToList()!;

        return GetPaginatedResponse(genres, request);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("/status")]
    public IActionResult Status()
    {
        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "NoMercy is running!",
            Data = "v1"
        });
    }

    [HttpGet]
    [Route("screensaver")]
    public async Task<IActionResult> Screensaver()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view screensaver");

        await using MediaContext mediaContext = new();
        List<Image> data = await mediaContext.Images
            .AsNoTracking()
            .Where(image => image.Movie.Library.LibraryUsers.FirstOrDefault(u => u.UserId == userId) != null ||
                            image.Tv.Library.LibraryUsers.FirstOrDefault(u => u.UserId == userId) != null
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

        IEnumerable<Image> tvCollection = data.Where(image => image is { Type: "backdrop", Tv: not null })
            .DistinctBy(image => image.TvId);
        IEnumerable<Image> movieCollection = data.Where(image => image is { Type: "backdrop", Movie: not null })
            .DistinctBy(image => image.MovieId);
        IEnumerable<Image> logos = data.Where(image => image is { Type: "logo" });

        return Ok(new ScreensaverDto
        {
            Data = tvCollection
                .Select(image => new ScreensaverDataDto(image, logos, "tv"))
                .Concat(movieCollection.Select(image => new ScreensaverDataDto(image, logos, "movie")))
                .Where(image => image.Meta?.Logo != null)
                .Randomize()
        });
    }
}
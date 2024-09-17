using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Api.Controllers.V1.Dashboard.DTO;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Data.Repositories;
using NoMercy.Database.Models;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.V1.Media;

[ApiController]
[Tags("Media Libraries")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/libraries")]
public class LibrariesController : BaseController
{
    private readonly ILibraryRepository _libraryRepository;

    public LibrariesController(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Libraries()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view libraries");

        IQueryable<Library> libraries = _libraryRepository.GetLibrariesAsync(userId);

        List<LibrariesResponseItemDto> response = libraries
            .Select(library => new LibrariesResponseItemDto(library))
            .ToList();

        return Ok(new LibrariesDto
        {
            Data = response.OrderBy(library => library.Order)
        });
    }

    [HttpGet]
    [Route("{libraryId:ulid}")]
    public async Task<IActionResult> Library(Ulid libraryId, [FromQuery] PageRequestDto request)
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view library");

        string language = Language();

        IEnumerable<Movie> movies = _libraryRepository
            .GetLibraryMoviesAsync(userId, libraryId, language, request.Take, request.Page);
        IEnumerable<Tv> shows = _libraryRepository
            .GetLibraryShowsAsync(userId, libraryId, language, request.Take, request.Page);

        if (request.Version != "lolomo")
        {
            IOrderedEnumerable<LibraryResponseItemDto> concat = movies
                .Select(movie => new LibraryResponseItemDto(movie))
                .Concat(shows.Select(tv => new LibraryResponseItemDto(tv)))
                .OrderBy(item => item.TitleSort);

            return GetPaginatedResponse(concat, request);
        }

        string[] numbers = ["*", "#", "'", "\"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
        string[] letters =
        [
            "#", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
            "U", "V", "W", "X", "Y", "Z"
        ];

        return Ok(new LoloMoResponseDto<LibraryResponseItemDto>
        {
            Data = letters.Select(genre => new LoloMoRowDto<LibraryResponseItemDto>
            {
                Title = genre,
                Id = genre,
                Items = movies.Select(movie => new LibraryResponseItemDto(movie))
                    .Where(item =>
                        genre == "#" ? numbers.Any(p => item.Title.StartsWith(p)) : item.Title.StartsWith(genre))
                    .Concat(shows.Select(tv => new LibraryResponseItemDto(tv))
                        .Where(item =>
                            genre == "#" ? numbers.Any(p => item.Title.StartsWith(p)) : item.Title.StartsWith(genre)))
            })
        });
    }
}
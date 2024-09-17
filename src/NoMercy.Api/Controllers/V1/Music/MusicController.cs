using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Database;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.V1.Music;

[ApiController]
[ApiVersion(1.0)]
[Tags("Music")]
[Authorize]
[Route("api/v{version:apiVersion}/music")]
public class MusicController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view music");

        List<GenreRowDto<dynamic>> list = [];

        MediaContext mediaContext = new();

        TopMusicDto? favoriteArtist = CarouselResponseItemDto.GetFavoriteArtist(mediaContext, userId);
        TopMusicDto? favoriteAlbum = CarouselResponseItemDto.GetFavoriteAlbum(mediaContext, userId);
        TopMusicDto? favoritePlaylist = CarouselResponseItemDto.GetFavoritePlaylist(mediaContext, userId);

        List<CarouselResponseItemDto> favoriteArtists =
            await CarouselResponseItemDto.GetFavoriteArtists(mediaContext, userId);
        List<CarouselResponseItemDto> favoriteAlbums =
            await CarouselResponseItemDto.GetFavoriteAlbums(mediaContext, userId);

        List<CarouselResponseItemDto> playlists = await CarouselResponseItemDto.GetPlaylists(mediaContext, userId);
        List<CarouselResponseItemDto> latestArtists =
            await CarouselResponseItemDto.GetLatestArtists(mediaContext, userId);
        List<CarouselResponseItemDto>
            latestAlbums = await CarouselResponseItemDto.GetLatestAlbums(mediaContext, userId);


        list.Add(new GenreRowDto<dynamic>
        {
            Title = null,
            MoreLink = "",
            Items =
            [
                favoriteArtist,
                favoriteAlbum,
                favoritePlaylist
            ]
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Favorite Artists",
            MoreLink = "",
            Items = favoriteArtists
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Favorite Albums",
            MoreLink = "",
            Items = favoriteAlbums
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Playlists",
            MoreLink = "app.music.playlists",
            Items = playlists
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Artists",
            MoreLink = "app.music.artists",
            Items = latestArtists
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Albums",
            MoreLink = "app.music.albums",
            Items = latestAlbums!
        });

        return Ok(new HomeResponseDto<dynamic>
        {
            Data = list
        });
    }

    [HttpPost]
    [Route("search")]
    public IActionResult Search()
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to search music");

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("search/{query}/{Type}")]
    public IActionResult TypeSearch(string query, string type)
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to search music");

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("coverimage")]
    public IActionResult CoverImage()
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view cover images");

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("images")]
    public IActionResult Images()
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view images");

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}
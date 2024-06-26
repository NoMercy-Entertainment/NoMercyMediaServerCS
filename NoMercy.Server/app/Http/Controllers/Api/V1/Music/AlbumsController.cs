using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Http.Middleware;
using NoMercy.Server.app.Jobs;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[Tags("Music Albums")]
[Authorize]
[Route("api/v{Version:apiVersion}/music/albums")]
public class AlbumsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FilterRequest request)
    {
        List<AlbumsResponseItemDto> albums = [];
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        await foreach (var album in AlbumsResponseDto.GetAlbums(mediaContext, userId, request.Letter ?? "_"))
            albums.Add(new AlbumsResponseItemDto(album, HttpContext.Request.Headers.AcceptLanguage[1]));

        var tracks = mediaContext.AlbumTrack
            .Where(albumTrack => albums.Select(a => a.Id).Contains(albumTrack.AlbumId))
            .Where(albumTrack => albumTrack.Track.Duration != null)
            .ToList();

        foreach (var album in albums) album.Tracks = tracks.Count(track => track.AlbumId == album.Id);

        return Ok(new AlbumsResponseDto
        {
            Data = albums
                .Where(response => response.Tracks > 0)
        });
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Show(Guid id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var album = await AlbumResponseDto.GetAlbum(mediaContext, userId, id);

        if (album is null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Album not found"
            });

        return Ok(new AlbumResponseDto
        {
            Data = new AlbumResponseItemDto(album, HttpContext.Request.Headers.AcceptLanguage[1])
        });
    }

    [HttpPost]
    [Route("{id:guid}/like")]
    public async Task<IActionResult> Like(Guid id, [FromBody] LikeRequestDto request)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var album = await mediaContext.Albums
            .AsNoTracking()
            .Where(album => album.Id == id)
            .FirstOrDefaultAsync();

        if (album is null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Album not found"
            });

        if (request.Value)
        {
            await mediaContext.AlbumUser
                .Upsert(new AlbumUser(album.Id, userId))
                .On(m => new { m.AlbumId, m.UserId })
                .WhenMatched(m => new AlbumUser
                {
                    AlbumId = m.AlbumId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var tvUser = await mediaContext.AlbumUser
                .Where(tvUser => tvUser.AlbumId == album.Id && tvUser.UserId == userId)
                .FirstOrDefaultAsync();

            if (tvUser is not null) mediaContext.AlbumUser.Remove(tvUser);

            await mediaContext.SaveChangesAsync();
        }

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto()
        {
            QueryKey = ["music", "albums", album.Id]
        });

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                album.Name,
                request.Value ? "liked" : "unliked"
            }
        });
    }

    [HttpPost]
    [Route("{id:guid}/rescan")]
    public async Task<IActionResult> Like(Guid id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescan started",
            Args = []
        });
    }
}
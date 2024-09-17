using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Api.Controllers.V1.Dashboard.DTO;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Api.Middleware;
using NoMercy.Data.Repositories;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Jobs;
using NoMercy.MediaProcessing.Jobs.MediaJobs;
using NoMercy.Networking;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Dashboard;

[ApiController]
[Tags("Dashboard Libraries")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard/libraries", Order = 10)]
public class LibrariesController : BaseController
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IEncoderRepository _encoderRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ILanguageRepository _languageRepository;

    public LibrariesController(ILibraryRepository libraryRepository, IEncoderRepository encoderRepository,
        IFolderRepository folderRepository, ILanguageRepository languageRepository)
    {
        _libraryRepository = libraryRepository;
        _encoderRepository = encoderRepository;
        _folderRepository = folderRepository;
        _languageRepository = languageRepository;
    }

    [HttpGet]
    public Task<IActionResult> Index()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return Task.FromResult(UnauthorizedResponse("You do not have permission to view libraries"));
        IQueryable<Library> libraries = _libraryRepository.GetLibrariesAsync(userId);
        return Task.FromResult<IActionResult>(Ok(new LibrariesDto
        {
            Data = libraries.Select(library => new LibrariesResponseItemDto(library))
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Store()
    {
        Guid userId = User.UserId();
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to create a new library");
        try
        {
            await using MediaContext mediaContext = new();
            int libraries = await mediaContext.Libraries.CountAsync();
            Library library = new()
            {
                Id = Ulid.NewUlid(),
                Title = $"Library {libraries}",
                AutoRefreshInterval = 30,
                ChapterImages = true,
                ExtractChapters = true,
                ExtractChaptersDuring = true,
                PerfectSubtitleMatch = true,
                Realtime = true,
                SpecialSeasonName = "Specials",
                Type = "",
                Order = 99
            };
            await _libraryRepository.AddLibraryAsync(library, userId);
            return Ok(new StatusResponseDto<Library>
            {
                Status = "ok", Data = library, Message = "Successfully created a new library.", Args = []
            });
        }
        catch (Exception e)
        {
            return Ok(new StatusResponseDto<Library>
            {
                Status = "error", Message = "Something went wrong creating a new library: {0}", Args = [e.Message]
            });
        }
    }

    [HttpPatch]
    [Route("{id:ulid}")]
    public async Task<IActionResult> Update(Ulid id, [FromBody] LibraryUpdateRequest request)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to update the library");

        Library? library = await _libraryRepository.GetLibraryByIdAsync(id);
        if (library is null)
            return NotFound(new StatusResponseDto<string> { Status = "error", Data = "Library not found" });

        try
        {
            library.Title = request.Title;
            library.PerfectSubtitleMatch = request.PerfectSubtitleMatch;
            library.Realtime = request.Realtime;
            library.SpecialSeasonName = request.SpecialSeasonName;
            library.Type = request.Type;
            library.LanguageLibraries.Clear();
            List<Language> languages = await _languageRepository.GetLanguagesAsync();
            foreach (string subtitle in request.Subtitles)
            {
                Language? language = languages.FirstOrDefault(l => l.Iso6391 == subtitle);
                if (language is null) continue;
                library.LanguageLibraries.Add(new LanguageLibrary { LibraryId = library.Id, LanguageId = language.Id });
            }

            await _libraryRepository.UpdateLibraryAsync(library);
        }
        catch (Exception e)
        {
            return Ok(new StatusResponseDto<string>
            {
                Status = "error", Message = "Something went wrong updating the library: {0}", Args = [e.Message]
            });
        }

        try
        {
            List<Folder> folders = await _folderRepository.GetFoldersByLibraryIdAsync(request.FolderLibrary);
            FolderLibrary[] folderLibraries = folders.Select(folder => new FolderLibrary
            {
                LibraryId = library.Id, FolderId = folder.Id
            }).ToArray();
            await _folderRepository.AddFolderLibraryAsync(folderLibraries);
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong updating the library folders: {0}",
                Args = [e.Message]
            });
        }

        try
        {
            List<LanguageLibrary> languages = await _languageRepository.GetLanguagesAsync(request.Subtitles);
            LanguageLibrary[] languageLibraries = languages.Select(language => new LanguageLibrary
            {
                LibraryId = library.Id, LanguageId = language.LanguageId
            }).ToArray();
            await _libraryRepository.AddLanguageLibraryAsync(languageLibraries);
        }
        catch (Exception e)
        {
            return Ok(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong updating the library subtitles: {0}",
                Args = [e.Message]
            });
        }

        try
        {
            List<Data.Repositories.FolderDto> folders = _libraryRepository.GetFoldersAsync();
            List<EncoderProfileFolder> encoderProfileFolders = [];

            foreach (Data.Repositories.FolderDto folder in folders)
                encoderProfileFolders.AddRange(folder.EncoderProfiles.Select(profile =>
                    new EncoderProfileFolder { FolderId = folder.Id, EncoderProfileId = profile }));

            await _libraryRepository.AddEncoderProfileFolderAsync(encoderProfileFolders);
        }
        catch (Exception e)
        {
            Logger.App(e);
            return Ok(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong updating the library encoder profiles: {0}",
                Args = [e.Message]
            });
        }

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok", Message = "Successfully updated {0} library.", Args = [library.Title]
        });
    }

    [HttpDelete]
    [Route("{id:ulid}")]
    public async Task<IActionResult> Delete(Ulid id)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to delete the library");

        Library? library = await _libraryRepository.GetLibraryByIdAsync(id);

        if (library is null)
            return Ok(new StatusResponseDto<string>
            {
                Status = "error", Message = "Library {0} does not exist.", Args = [id.ToString()]
            });
        try
        {
            await _libraryRepository.DeleteLibraryAsync(library);
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully deleted {0} library.", Args = [library.Title]
            });
        }
        catch (Exception e)
        {
            return Ok(new StatusResponseDto<string>
            {
                Status = "error", Message = "Something went wrong deleting the library: {0}", Args = [e.Message]
            });
        }
    }

    [HttpPatch]
    [Route("sort")]
    public async Task<IActionResult> Sort(Ulid id, [FromBody] LibrarySortRequest request)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to sort the libraries");

        List<Library> libraries = await _libraryRepository.GetAllLibrariesAsync();

        if (libraries.Count == 0)
            return Ok(new StatusResponseDto<string> { Status = "error", Message = "No libraries exist.", Args = [] });

        try
        {
            foreach (LibrarySortRequestItem item in request.Libraries)
            {
                Library? lib = libraries.FirstOrDefault(l => l.Id == item.Id);
                if (lib is null) continue;
                lib.Order = item.Order;
            }

            await _libraryRepository.UpdateLibraryAsync(libraries.First());
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully sorted libraries.", Args = []
            });
        }
        catch (Exception e)
        {
            return Problem(title: "Internal Server Error.",
                detail: $"Something went wrong sorting the libraries {e.Message}", instance: HttpContext.Request.Path,
                statusCode: StatusCodes.Status403Forbidden, type: "/docs/errors/internal-server-error");
        }
    }

    [HttpPost]
    [Route("rescan")]
    public async Task<IActionResult> RescanAll()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to rescan all libraries");

        List<Library> librariesList = await _libraryRepository.GetAllLibrariesAsync();

        if (librariesList.Count == 0)
            return NotFound(new StatusResponseDto<List<string?>>
            {
                Status = "error", Message = "No libraries found to rescan.", Args = []
            });

        List<string?> titles = [];
        foreach (Library library in librariesList)
        {
            JobDispatcher jobDispatcher = new();
            jobDispatcher.DispatchJob<RescanLibraryJob>(library.Id);
        }

        return Ok(new StatusResponseDto<List<string?>>
        {
            Status = "ok", Data = titles, Message = "Rescanning all libraries."
        });
    }

    [HttpPost]
    [Route("{id:ulid}/rescan")]
    public async Task<IActionResult> Rescan(Ulid id)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to rescan the library");

        JobDispatcher jobDispatcher = new();
        jobDispatcher.DispatchJob<RescanLibraryJob>(id);

        return Ok(new StatusResponseDto<List<dynamic>>
        {
            Status = "ok", Message = "Rescanning {0} library.", Args = [id]
        });
    }

    [HttpPost]
    [Route("{id:ulid}/folders")]
    public async Task<IActionResult> AddFolder(Ulid id, [FromBody] FolderRequest request)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to add a new folder to the library");

        Library? library = await _libraryRepository.GetLibraryByIdAsync(id);
        if (library is null)
            return NotFoundResponse("Library not found");

        try
        {
            Folder folder = new() { Id = Ulid.NewUlid(), Path = request.Path };
            await _folderRepository.AddFolderAsync(folder);
            DynamicStaticFilesMiddleware.AddPath(library.Id, folder.Path);
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error", Message = "Something went wrong adding the folder: {0}", Args = [e.Message]
            });
            return UnprocessableEntity(
                $"Something went wrong adding a new folder {id} to {library} library: {e.Message}");
        }

        try
        {
            Folder? folder = await _folderRepository.GetFolderByPathAsync(request.Path);
            if (folder is null)
                return NotFound(new StatusResponseDto<string>
                {
                    Status = "error", Message = "Folder {0} does not exist.", Args = [id.ToString()]
                });

            FolderLibrary folderLibrary = new() { LibraryId = library.Id, FolderId = folder.Id };
            await _folderRepository.AddFolderLibraryAsync(folderLibrary);
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong adding a new folder to {0} library: {1}",
                Args = [id.ToString(), e.Message]
            });
        }

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok", Message = "Successfully added folder to {0} library.", Args = [id.ToString()]
        });
    }

    [HttpPatch]
    [Route("{id:ulid}/folders/{folderId:ulid}")]
    public async Task<IActionResult> UpdateFolder(Ulid id, Ulid folderId, [FromBody] FolderRequest request)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to update the library folder");

        Folder? folder = await _folderRepository.GetFolderByIdAsync(folderId);
        if (folder is null)
            return NotFound(new StatusResponseDto<string> { Status = "error", Data = "Folder not found" });

        try
        {
            folder.Path = request.Path;
            await _folderRepository.UpdateFolderAsync(folder);
            DynamicStaticFilesMiddleware.RemovePath(folder.Id);
            DynamicStaticFilesMiddleware.AddPath(id, folder.Path);
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully updated folder {0}.", Args = [folder.Path]
            });
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong updating the library folder: {0}",
                Args = [e.Message]
            });
        }
    }

    [HttpDelete]
    [Route("{id:ulid}/folders/{folderId:ulid}")]
    public async Task<IActionResult> DeleteFolder(Ulid id, Ulid folderId)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to delete the library folder");

        Folder? folder = await _folderRepository.GetFolderByIdAsync(folderId);
        if (folder is null)
            return NotFound(new StatusResponseDto<string> { Status = "error", Data = "Folder not found" });

        try
        {
            await _folderRepository.DeleteFolderAsync(folder);
            DynamicStaticFilesMiddleware.RemovePath(folder.Id);
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully deleted folder {0}.", Args = [folder.Path]
            });
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong deleting the library folder: {0}",
                Args = [e.Message]
            });
        }
    }

    [HttpPost]
    [Route("{id:ulid}/folders/{folderId:ulid}/encoder_profiles")]
    public async Task<IActionResult> AddEncoderProfile(Ulid id, Ulid folderId, [FromBody] ProfilesRequest request)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to add a new encoder profile to the folder");

        Folder? folder = await _folderRepository.GetFolderByIdAsync(folderId);
        if (folder is null)
            return NotFound(new StatusResponseDto<string> { Status = "error", Data = "Folder not found" });

        try
        {
            EncoderProfileFolder[] encoderProfileFolder = request.Profiles.Select(profile =>
                new EncoderProfileFolder { FolderId = folder.Id, EncoderProfileId = Ulid.Parse(profile) }).ToArray();

            await _libraryRepository.AddEncoderProfileFolderAsync(encoderProfileFolder);
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully added encoder profile to {0} folder.", Args = [id.ToString()]
            });
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong adding the encoder profile: {0}",
                Args = [e.Message]
            });
        }
    }

    [HttpDelete]
    [Route("{id:ulid}/folders/{folderId:ulid}/encoder_profiles/{encoderProfileId:ulid}")]
    public async Task<IActionResult> DeleteEncoderProfile(Ulid id, Ulid profileId)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to delete the encoder profile");

        EncoderProfile? encoderProfile = await _encoderRepository.GetEncoderProfileByIdAsync(profileId);
        if (encoderProfile is null)
            return NotFound(new StatusResponseDto<string> { Status = "error", Data = "Encoder profile not found" });

        try
        {
            await _encoderRepository.DeleteEncoderProfileAsync(encoderProfile);
            return Ok(new StatusResponseDto<string>
            {
                Status = "ok", Message = "Successfully deleted encoder profile {0}.", Args = [encoderProfile.Name]
            });
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Something went wrong deleting the encoder profile: {0}",
                Args = [e.Message]
            });
        }
    }
}
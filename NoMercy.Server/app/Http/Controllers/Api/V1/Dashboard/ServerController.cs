using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Helpers.Monitoring;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Http.Clients;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Management")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/server", Order = 10)]
public class ServerController : Controller
{
    private IHostApplicationLifetime ApplicationLifetime { get; }

    public ServerController(IHostApplicationLifetime appLifetime)
    {
        ApplicationLifetime = appLifetime;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }

    [HttpGet]
    [Route("setup")]
    public async Task<StatusResponseDto<SetupResponseDto>> Setup()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        List<Library> libraries = await mediaContext.Libraries
            .Where(library => library.LibraryUsers
                .Any(libraryUser => libraryUser.UserId == userId)
            )
            .Include(library => library.FolderLibraries)
            .ThenInclude(folderLibrary => folderLibrary.Folder)
            .ThenInclude(folder => folder.EncoderProfileFolder)
            .ThenInclude(encoderProfileFolder => encoderProfileFolder.EncoderProfile)
            .Include(library => library.LibraryUsers)
            .ThenInclude(libraryUser => libraryUser.User)
            .ToListAsync();

        int libraryCount = libraries.Count;

        int folderCount = libraries
            .SelectMany(library => library.FolderLibraries)
            .Select(folderLibrary => folderLibrary.Folder)
            .Count();

        int encoderProfileCount = libraries
            .SelectMany(library => library.FolderLibraries)
            .Select(folderLibrary => folderLibrary.Folder)
            .Count(folder => folder.EncoderProfileFolder.Count > 0);

        return new StatusResponseDto<SetupResponseDto>
        {
            Status = "ok",
            Data = new SetupResponseDto
            {
                SetupComplete = libraryCount > 0
                                && folderCount > 0
                                && encoderProfileCount > 0
            },
        };
    }

    [HttpPost]
    [Route("start")]
    public IActionResult StartServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("stop")]
    public IActionResult StopServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("restart")]
    public IActionResult RestartServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("shutdown")]
    public IActionResult Shutdown()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("loglevel")]
    public IActionResult LogLevel(LogLevel level)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        Logger.SetLogLevel(level);

        return Content("Log level set to " + level);
    }

    [HttpPost]
    [Route("directorytree")]
    public StatusResponseDto<List<DirectoryTreeDto>> DirectoryTree([FromBody] PathRequest request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        string? path = request.Path ?? request.Folder;

        List<DirectoryTreeDto> array = [];

        if (string.IsNullOrEmpty(path) || path == "/")
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform
                    .Windows))
            {
                DriveInfo[] driveInfo = DriveInfo.GetDrives();
                array = driveInfo.Select(d => CreateDirectoryTreeDto("", d.RootDirectory.ToString())).ToList();

                return new StatusResponseDto<List<DirectoryTreeDto>>
                {
                    Status = "ok",
                    Data = array
                };
            }

            path = "/";
        }

        Logger.App(path);

        if (!Directory.Exists(path))
        {
            return new StatusResponseDto<List<DirectoryTreeDto>>
            {
                Status = "ok",
                Data = array
            };
        }

        try
        {
            var directories = Directory.GetDirectories(path);
            array = directories.Select(d => CreateDirectoryTreeDto(path, d)).ToList();
        }
        catch (Exception ex)
        {
            return new StatusResponseDto<List<DirectoryTreeDto>>
            {
                Status = "error",
                Message = ex.Message
            };
        }

        return new StatusResponseDto<List<DirectoryTreeDto>>
        {
            Status = "ok",
            Data = array
        };
    }

    private DirectoryTreeDto CreateDirectoryTreeDto(string parent, string path)
    {
        string fullPath = Path.Combine(parent, path);

        FileInfo fileInfo = new FileInfo(fullPath);

        string type = fileInfo.Attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";

        string newPath = string.IsNullOrEmpty(fileInfo.Name)
            ? path
            : fileInfo.Name;
        
        string parentPath = string.IsNullOrEmpty(parent)
            ? "/"
            : Path.Combine(fullPath, @"..\..");
        
        return new DirectoryTreeDto
        {
            Path = newPath,
            Parent = parentPath,
            FullPath = fullPath.Replace(@"..\", ""),
            Mode = (int)fileInfo.Attributes,
            Size = type == "file" ? int.Parse(fileInfo.Length.ToString()) : null,
            Type = type
        };
    }

    public static string GetDeviceName()
    {
        MediaContext mediaContext = new();
        Configuration? device = mediaContext.Configuration?.FirstOrDefault(device => device.Key == "server_name");
        return device?.Value ?? Environment.MachineName;
    }

    [HttpGet]
    [Route("info")]
    public ServerInfoDto ServerInfo()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        return new ServerInfoDto
        {
            Server = GetDeviceName(),
            Cpu = SystemInfo.Cpu,
            Os = SystemInfo.Platform.ToTitleCase(),
            Arch = SystemInfo.Architecture,
            Version = SystemInfo.Version,
            BootTime = SystemInfo.StartTime,
        };
    }

    [HttpGet]
    [Route("resources")]
    public ResourceInfoDto Resources()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        double totalCpu = 0.0;
        double totalMemory = 0.0;
        foreach (var aProc in Process.GetProcesses())
        {
            totalMemory += aProc.WorkingSet64 / 1024.0;
        }
        
        return new ResourceInfoDto
        {
            Cpu = totalCpu / Environment.ProcessorCount,
            Ram = totalMemory / 1024.0 / 1024.0,
            Storage = StorageMonitor.Main(),
        };
    }

    [HttpPatch]
    [Route("info")]
    public async Task<StatusResponseDto<string>> Update([FromBody] ServerUpdateRequest request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        await using MediaContext mediaContext = new();
        var configuration = await mediaContext.Configuration
            .AsTracking()
            .FirstOrDefaultAsync(configuration => configuration.Key == "server_name");

        try
        {
            if (configuration == null)
            {
                configuration = new Configuration
                {
                    Key = "server_name",
                    Value = request.Name,
                    ModifiedBy = userId,
                };
                await mediaContext.Configuration.AddAsync(configuration);
            }
            else
            {
                configuration.Value = request.Name;
                configuration.ModifiedBy = userId;
            }

            await mediaContext.SaveChangesAsync();

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://api-dev.nomercy.tv/v1/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, "server/name")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["id"] = SystemInfo.DeviceId.ToString(),
                    ["server_name"] = request.Name
                })
            };

            var response = client
                .SendAsync(httpRequestMessage)
                .Result.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<StatusResponseDto<string>>(response);

            return new StatusResponseDto<string>
            {
                Status = data.Status,
                Message = data.Message,
                Args = [],
            };
        }
        catch (Exception e)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Server name could not be updated: {0}",
                Args = [e.Message],
            };
        }
    }

    [HttpGet]
    [Route("paths")]
    public ServerPathsDto[] ServerPaths()
    {
        return
        [
            new ServerPathsDto
            {
                Key = "Cache",
                Value = AppFiles.CachePath
            },
            new ServerPathsDto
            {
                Key = "Logs",
                Value = AppFiles.LogPath
            },
            new ServerPathsDto
            {
                Key = "Metadata",
                Value = AppFiles.MetadataPath
            },
            new ServerPathsDto
            {
                Key = "Transcodes",
                Value = AppFiles.TranscodePath
            },
            new ServerPathsDto
            {
                Key = "Configs",
                Value = AppFiles.ConfigPath
            }
        ];
    }

    [HttpGet]
    [Route("/files/${depth:int}/${path:required}")]
    public async Task<List<MediaFolder>?> Files(string path, int depth)
    {
        Logger.App($@"Files: {path}");
        MediaScan mediaScan = new();

        List<MediaFolder>? folders = await mediaScan
            .EnableFileListing()
            .Process(path, depth);

        mediaScan.Dispose();

        return folders;
    }

    [HttpPatch]
    [Route("workers/{worker}/{count:int:min(0)}")]
    public async Task<IActionResult> UpdateWorkers(string worker, int count)
    {
        if (await QueueRunner.SetWorkerCount(worker, count))
        {
            return Ok($"{worker} worker count set to {count}");
        }

        return BadRequest($"{worker} worker count could not be set to {count}");
    }
}

public class ServerInfoDto
{
    [JsonProperty("server")] public string Server { get; set; }
    [JsonProperty("cpu")] public string? Cpu { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("arch")] public string Arch { get; set; }
    [JsonProperty("version")] public string? Version { get; set; }
    [JsonProperty("bootTime")] public DateTime BootTime { get; set; }
}

public class ServerPathsDto
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("value")] public string Value { get; set; }
}

public class DirectoryRequest
{
    [JsonProperty("path")] public string Path { get; set; }
}

public class PathRequest
{
    [JsonProperty("path")] public string? Path { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
}

public class DirectoryTreeDto
{
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("mode")] public int Mode { get; set; }
    [JsonProperty("size")] public int? Size { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("parent")] public string Parent { get; set; }
    [JsonProperty("full_path")] public string FullPath { get; set; }
}

public class SetupResponseDto
{
    [JsonProperty("setup_complete")] public bool SetupComplete { get; set; }
}

public class ServerUpdateRequest
{
    [JsonProperty("name")] public string Name { get; set; }
}

public class ResourceInfoDto
{
    [JsonProperty("cpu")] public double Cpu { get; set; }
    [JsonProperty("ram")] public double Ram { get; set; }
    [JsonProperty("storage")] public List<ResourceMonitorDto> Storage { get; set; }
}
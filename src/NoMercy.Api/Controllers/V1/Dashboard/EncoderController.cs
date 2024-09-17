using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Data.Repositories;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Encoder Profiles")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard/encoderprofiles", Order = 10)]
public class EncoderController : BaseController
{
    private readonly IEncoderRepository _encoderRepository;

    public EncoderController(IEncoderRepository encoderRepository)
    {
        _encoderRepository = encoderRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to view encoder profiles");

        await using MediaContext mediaContext = new();
        List<EncoderProfile> profiles = await _encoderRepository.GetEncoderProfilesAsync();

        List<EncoderProfileDto> encoderProfiles = profiles
            .Where(profile => profile.Param != null)
            .Select(profile => new EncoderProfileDto
            {
                Id = profile.Id.ToString(),
                Name = profile.Name,
                Container = profile.Container,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
                Params = JsonConvert.DeserializeObject<ParamsDto>(profile.Param!)
            })
            .ToList();

        return Ok(encoderProfiles);

        // List<EncoderProfileDto> encoderProfiles = [];
        //
        // foreach (EncoderProfile profile in profiles)
        // {
        //     if (profile.Param == null) continue;
        //
        //     ParamsDto? paramJson = JsonConvert.DeserializeObject<ParamsDto>(profile.Param);
        //
        //     EncoderProfileDto profileDto = new()
        //     {
        //         Id = profile.Id.ToString(),
        //         Name = profile.Name,
        //         Container = profile.Container,
        //         CreatedAt = profile.CreatedAt,
        //         UpdatedAt = profile.UpdatedAt,
        //         Params = paramJson is not null
        //             ? new ParamsDto
        //             {
        //                 Width = paramJson.Width,
        //                 Crf = paramJson.Crf,
        //                 Preset = paramJson.Preset,
        //                 Profile = paramJson.Profile,
        //                 Codec = paramJson.Codec,
        //                 Audio = paramJson.Audio
        //             }
        //             : null
        //     };
        //     encoderProfiles.Add(profileDto);
        // }
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to create encoder profiles");

        try
        {
            await using MediaContext mediaContext = new();
            int encoderProfiles = await _encoderRepository.GetEncoderProfileCountAsync();

            EncoderProfile profile = new()
            {
                Id = Ulid.NewUlid(),
                Name = $"Profile {encoderProfiles}",
                Container = "mp4",
                Param = JsonConvert.SerializeObject(new ParamsDto
                {
                    Width = 1920,
                    Crf = 23,
                    Preset = "medium",
                    Profile = "main",
                    Codec = "libx264",
                    Audio = "aac"
                })
            };

            await _encoderRepository.AddEncoderProfileAsync(profile);

            return Ok(new StatusResponseDto<EncoderProfile>
            {
                Status = "ok",
                Data = profile,
                Message = "Successfully created a new encoder profile.",
                Args = []
            });
        }
        catch (Exception e)
        {
            return Ok(new StatusResponseDto<EncoderProfile>()
            {
                Status = "error",
                Message = "Something went wrong creating a new library: {0}",
                Args = [e.Message]
            });
        }
    }

    [HttpDelete]
    [Route("{id:ulid}")]
    public async Task<IActionResult> Destroy(Ulid id)
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to remove encoder profiles");

        EncoderProfile? profile = await _encoderRepository.GetEncoderProfileByIdAsync(id);

        if (profile == null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Data = "Encoder profile not found"
            });

        await _encoderRepository.DeleteEncoderProfileAsync(profile);

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Data = "Profile removed"
        });
    }

    [HttpGet]
    [Route("containers")]
    public IActionResult Containers()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to remove encoder profiles");

        IEnumerable<ContainerDto> containers = [];
        // IEnumerable<ContainerDto> containers = BaseContainer.AvailableContainers
        //     .Select(container => new ContainerDto
        //     {
        //         Label = container.Name,
        //         Value = container.Name,
        //         Type = container.Type,
        //         IsDefault = container.IsDefault
        //     });

        return Ok(new DataResponseDto<ContainerDto[]>
        {
            Data = containers.ToArray()
        });
    }
}
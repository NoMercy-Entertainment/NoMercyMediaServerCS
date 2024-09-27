using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Api.Controllers.V1.Dashboard.DTO;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Api.Controllers.V1.Music;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Queue;

namespace NoMercy.Api.Controllers.V1.Dashboard;

[ApiController]
[Tags("Dashboard Configuration")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard/configuration", Order = 10)]
public class ConfigurationController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to view configuration");

        return Ok(new ConfigDto
        {
            Data = new ConfigDtoData
            {
                InternalServerPort = Config.InternalServerPort,
                ExternalServerPort = Config.ExternalServerPort,
                QueueWorkers = Config.QueueWorkers.Value,
                EncoderWorkers = Config.EncoderWorkers.Value,
                CronWorkers = Config.CronWorkers.Value,
                DataWorkers = Config.DataWorkers.Value,
                ImageWorkers = Config.ImageWorkers.Value,
                RequestWorkers = Config.RequestWorkers.Value,
                ServerName = DeviceName()
            }
        });
    }
    
    [NonAction]
    private static string DeviceName()
    {
        MediaContext mediaContext = new();
        Configuration? device = mediaContext.Configuration.FirstOrDefault(device => device.Key == "serverName");
        return device?.Value ?? Environment.MachineName;
    }

    [HttpPost]
    public IActionResult Store()
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to store configuration");

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] ConfigDtoData request)
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to update configuration");
        
        if (request.InternalServerPort != 0)
        {
            Config.InternalServerPort = request.InternalServerPort;
            MediaContext mediaContext = new();
            await mediaContext.Configuration.Upsert(new Configuration
                {
                    Key = "InternalServerPort",
                    Value = request.InternalServerPort.ToString(),
                    ModifiedBy = User.UserId()
                })
                .On(e => e.Key)
                .WhenMatched((o, n) =>new Configuration
                {
                    Id = o.Id,
                    Value = n.Value,
                    UpdatedAt = n.UpdatedAt
                })
                .RunAsync();
        }
        
        if (request.ExternalServerPort != 0)
        {
            Config.ExternalServerPort = request.ExternalServerPort;
            MediaContext mediaContext = new();
            await mediaContext.Configuration.Upsert(new Configuration
                {
                    Key = "ExternalServerPort",
                    Value = request.ExternalServerPort.ToString(),
                    ModifiedBy = User.UserId()
                })
                .On(e => e.Key)
                .WhenMatched((o, n) =>new Configuration
                {
                    Id = o.Id,
                    Value = n.Value,
                    UpdatedAt = n.UpdatedAt
                })
                .RunAsync();
        }
        
        if (request.QueueWorkers is not null)
        {
            Config.QueueWorkers = new(Config.QueueWorkers.Key, (int)request.QueueWorkers);
            await QueueRunner.SetWorkerCount(Config.QueueWorkers.Key, (int)request.QueueWorkers);
        }
        if (request.EncoderWorkers is not null)
        {
            Config.EncoderWorkers = new(Config.EncoderWorkers.Key, (int)request.EncoderWorkers);
            await QueueRunner.SetWorkerCount(Config.EncoderWorkers.Key, (int)request.EncoderWorkers);
        }
        if (request.CronWorkers is not null)
        {
            Config.CronWorkers = new(Config.CronWorkers.Key, (int)request.CronWorkers);
            await QueueRunner.SetWorkerCount(Config.CronWorkers.Key, (int)request.CronWorkers);
        }
        if (request.DataWorkers is not null)
        {
            Config.DataWorkers = new(Config.DataWorkers.Key, (int)request.DataWorkers);
            await QueueRunner.SetWorkerCount(Config.DataWorkers.Key, (int)request.DataWorkers);
        }
        if (request.ImageWorkers is not null)
        {
            Config.ImageWorkers = new(Config.ImageWorkers.Key, (int)request.ImageWorkers);
            await QueueRunner.SetWorkerCount(Config.ImageWorkers.Key, (int)request.ImageWorkers);
        }
        if (request.RequestWorkers is not null)
        {
            Config.RequestWorkers = new(Config.RequestWorkers.Key, (int)request.RequestWorkers);
            await QueueRunner.SetWorkerCount(Config.RequestWorkers.Key, (int)request.RequestWorkers);
        }
        
        if (request.ServerName is not null)
        {
            MediaContext mediaContext = new();
            await mediaContext.Configuration.Upsert(new Configuration
            {
                Key = "serverName",
                Value = request.ServerName,
                ModifiedBy = User.UserId()
            })
            .On(e => e.Key)
            .WhenMatched((o, n) =>new Configuration
            {
                Id = o.Id,
                Value = request.ServerName,
                UpdatedAt = n.UpdatedAt
            })
            .RunAsync();
        }
        
        return Ok(new StatusResponseDto<string>
        {
            Message = "Configuration updated successfully",
            Status = "success",
            Args = [],
        });
    }

    [HttpGet]
    [Route("languages")]
    public async Task<IActionResult> Languages()
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view languages");

        await using MediaContext context = new();
        List<Language> languages = await context.Languages
            .ToListAsync();

        return Ok(languages.Select(language => new LanguageDto
        {
            Id = language.Id,
            Iso6391 = language.Iso6391,
            EnglishName = language.EnglishName,
            Name = language.Name
        }).ToList());
    }

    [HttpGet]
    [Route("countries")]
    public async Task<IActionResult> Countries()
    {
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view countries");

        await using MediaContext context = new();
        List<Country> countries = await context.Countries
            .ToListAsync();

        return Ok(countries.Select(country => new CountryDto
        {
            Name = country.EnglishName,
            Code = country.Iso31661
        }).ToList());
    }
}
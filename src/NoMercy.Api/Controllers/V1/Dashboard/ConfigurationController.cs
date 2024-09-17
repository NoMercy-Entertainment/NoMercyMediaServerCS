using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Api.Controllers.V1.Music;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Networking;

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

        return Ok(new PlaceholderResponse()
        {
            Data = []
        });
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
    public IActionResult Update()
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to update configuration");

        return Ok(new PlaceholderResponse
        {
            Data = []
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
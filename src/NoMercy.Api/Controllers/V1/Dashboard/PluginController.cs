#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Helpers;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Plugins")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard/plugins", Order = 10)]
public class PluginController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to view plugins");

        // AniDBAnimeItem randomAnime = await AniDbRandomAnime.GetRandomAnime();

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Plugins loaded successfully"
        });
    }

    [HttpGet]
    [Route("credentials")]
    public IActionResult Credentials()
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to view credentials");

        UserPass? aniDb = CredentialManager.Credential("AniDb");

        if (aniDb == null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "No credentials found for AniDb"
            });

        return Ok(new AniDbCredentialsResponse
        {
            Key = "AniDb",
            Username = aniDb.Username,
            ApiKey = aniDb.ApiKey
        });
    }

    [HttpPost]
    [Route("credentials")]
    public IActionResult Credentials([FromBody] AniDbCredentialsRequest request)
    {
        if (!User.IsOwner())
            return UnauthorizedResponse("You do not have permission to set credentials");

        UserPass? aniDb = CredentialManager.Credential(request.Key);
        CredentialManager.SetCredentials(request.Key, request.Username, request.Password ?? aniDb?.Password ?? "",
            request.ApiKey);

        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Credentials set successfully for {0}",
            Args = [request.Key]
        });
    }
}

public class AniDbCredentialsRequest
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("password")] public string? Password { get; set; }
    [JsonProperty("api_key")] public string ApiKey { get; set; }
}

public class AniDbCredentialsResponse
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("api_key")] public string? ApiKey { get; set; }
}
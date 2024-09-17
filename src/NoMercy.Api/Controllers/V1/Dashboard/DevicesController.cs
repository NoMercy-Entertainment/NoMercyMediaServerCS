using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.Music;
using NoMercy.Data.Repositories;
using NoMercy.Database.Models;
using NoMercy.Networking;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Devices")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard/devices", Order = 10)]
public class DevicesController : BaseController
{
    private readonly IDeviceRepository _deviceRepository;

    public DevicesController(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    [HttpGet]
    public Task<IActionResult> Index()
    {
        if (!User.IsModerator())
            return Task.FromResult(UnauthorizedResponse("You do not have permission to view devices"));

        IIncludableQueryable<Device, ICollection<ActivityLog>> devices = _deviceRepository.GetDevicesAsync();

        IEnumerable<DevicesDto> devicesDtos = devices
            .Select(x => new DevicesDto
            {
                Id = x.Id.ToString(),
                DeviceId = x.DeviceId,
                Browser = x.Browser,
                Os = x.Os,
                Device = x.Model,
                Type = x.Type,
                Name = x.Name,
                CustomName = x.CustomName,
                Version = x.Version,
                Ip = x.Ip,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                ActivityLogs = x.ActivityLogs
                    .Select(activityLog => new ActivityLogDto
                    {
                        Id = activityLog.Id,
                        Type = activityLog.Type,
                        Time = activityLog.Time,
                        CreatedAt = activityLog.CreatedAt,
                        UpdatedAt = activityLog.UpdatedAt,
                        UserId = activityLog.UserId,
                        DeviceId = activityLog.DeviceId.ToString()
                    })
            });

        return Task.FromResult<IActionResult>(Ok(devicesDtos.ToArray()));
    }

    [HttpPost]
    public IActionResult Create()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to create devices");

        // Add device creation logic here
        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpDelete]
    public IActionResult Destroy()
    {
        if (!User.IsModerator())
            return UnauthorizedResponse("You do not have permission to delete devices");

        // Add device deletion logic here
        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}

public class DevicesDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string? Device { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("custom_name")] public object? CustomName { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("activity_logs")] public IEnumerable<ActivityLogDto> ActivityLogs { get; set; }
}

public class ActivityLogDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("user_id")] public Guid UserId { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
}
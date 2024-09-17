using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Http;
using NoMercy.Database.Models;
using NoMercy.Networking;
using NoMercy.NmSystem;

namespace NoMercy.Api.Middleware;

public class AccessLogMiddleware(RequestDelegate next) {

    private readonly string[] _ignoredStartsWithRoutes =
    [
        "/images",
        "/swagger",
        "/index",
        "/styles",
        "/scripts"
    ];

    private readonly string[] _ignoreExact =
    [
        "/",
        "/api/v1/dashboard/logs"
    ];

    private readonly string[] _ignoreIfAuthenticated =
    [
        "/socket"
    ];

    private readonly string[] _ignoreIfGuest =
    [
        "/status"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldIgnoreRoute(context))
        {
            await next(context);
            return;
        }

        string path = HttpUtility.UrlDecode(context.Request.Path);
        string remoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        string? guid = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (guid == null || !Guid.TryParse(guid, out Guid userId) || userId == Guid.Empty)
        {
            Logger.Http($"Unknown: {remoteIpAddress}: {path}");
        }
        else if (ShouldIgnoreIfAuthenticated(path) || ShouldIgnoreFolder(path))
        {
            // Ignored authenticated route or folder
        }
        else
        {
            User? user = ClaimsPrincipleExtensions.Users.FirstOrDefault(x => x.Id == userId);
            Logger.Http($"{user?.Name ?? $"Unknown: {remoteIpAddress}:"}: {path}");
        }

        await next(context);
    }

    private bool ShouldIgnoreRoute(HttpContext context) {
        string path = context.Request.Path.ToString();
        return _ignoredStartsWithRoutes.Any(route => path.StartsWith(route)) 
            || _ignoreExact.Any(path.Equals) 
            || _ignoreIfGuest.Any(path.Equals);
    }

    private bool ShouldIgnoreIfAuthenticated(string path)
    {
        return _ignoreIfAuthenticated.Any(path.Equals);
    }

    private static bool ShouldIgnoreFolder(string path)
    {
        return ClaimsPrincipleExtensions.FolderIds.Any(x => path.StartsWith("/" + x));
    }
}
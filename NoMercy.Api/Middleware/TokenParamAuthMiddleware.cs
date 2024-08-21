using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NoMercy.Database.Models;
using NoMercy.Networking;

namespace NoMercy.Api.Middleware;

public class TokenParamAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers.Authorization = context.Request.Headers.Authorization.ToString().Split(",")[0];

        string url = context.Request.Path;

        if (!ClaimsPrincipleExtensions.FolderIds.Any(x => url.StartsWith("/" + x)) ||
            context.Request.Headers.Authorization.ToString().Contains("Bearer"))
        {
            await next(context);
            return;
        }

        string? claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(claim))
        {
            string jwt = context.Request.Query
                .FirstOrDefault(q => q.Key is "token" or "access_token").Value.ToString();

            if (string.IsNullOrEmpty(jwt))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            context.Request.Headers.Authorization = new StringValues("Bearer " + jwt);
        }
        else
        {
            Guid userId = Guid.Parse(claim);

            if (userId == Guid.Empty)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            User? user = ClaimsPrincipleExtensions.Users.FirstOrDefault(x => x.Id == userId);

            if (user is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        await next(context);
    }
}
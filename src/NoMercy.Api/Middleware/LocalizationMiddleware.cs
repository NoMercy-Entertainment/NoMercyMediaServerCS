using Microsoft.AspNetCore.Http;

namespace NoMercy.Api.Middleware;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string userLanguages = context.Request.Headers["Accept-Language"].ToString();

        // if the language string does not match the format "{language}-{country}" we add the uppercase version of the language
        if (!userLanguages.Contains("-")) userLanguages = userLanguages + "-" + userLanguages.ToUpper();

        string[]? firstLang = userLanguages.Split(',').FirstOrDefault()?.Split('-');

        if (firstLang is not null && firstLang.Length > 0)
            context.Request.Headers.AcceptLanguage = firstLang;
        else
            context.Request.Headers.AcceptLanguage = "en-US".Split('-');

        await _next(context);
    }
}
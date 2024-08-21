using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace NoMercy.Api.Middleware;

public class DynamicStaticFilesMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<Ulid, PhysicalFileProvider> Providers = new();

    public DynamicStaticFilesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.HasValue)
        {
            await _next(context);
            return;
        }

        string rootPath = context.Request.Path.ToString().Split("/")[1];
        if (rootPath == "api" || rootPath == "index.html" || rootPath.StartsWith("swagger") || rootPath == "images")
        {
            await _next(context);
            return;
        }

        try
        {
            Ulid share = Ulid.Parse(rootPath);

            if (Providers.TryGetValue(share, out PhysicalFileProvider? provider))
            {
                IFileInfo file =
                    provider.GetFileInfo(context.Request.Path.Value[context.Request.Path.Value.IndexOf('/', 1)..]);

                if (file.PhysicalPath != null)
                {
                    string? fullFilePath = file.PhysicalPath;
                    FileInfo fileInfo = new(fullFilePath);
                    long fileLength = fileInfo.Length;
                    string? fileExtension = Path.GetExtension(file.PhysicalPath)?.ToLower();

                    context.Response.ContentType = fileExtension switch
                    {
                        ".txt" => "text/plain",
                        ".html" => "text/html",
                        ".css" => "text/css",
                        ".js" => "application/javascript",
                        ".json" => "application/json",
                        ".xml" => "application/xml",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".webp" => "image/webp",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".bmp" => "image/bmp",
                        ".ico" => "image/x-icon",
                        ".svg" => "image/svg+xml",
                        ".mp3" => "audio/mpeg",
                        ".wav" => "audio/wav",
                        ".mp4" => "video/mp4",
                        ".mpeg" => "video/mpeg",
                        ".vtt" => "text/vtt",
                        ".srt" => "text/srt",
                        ".webm" => "video/webm",
                        _ => "application/octet-stream"
                    };

                    if (context.Request.Headers.TryGetValue("Range", out StringValues rangeValue))
                        try
                        {
                            RangeItemHeaderValue range = RangeHeaderValue.Parse(rangeValue.ToString()).Ranges.First();
                            long start = range.From ?? 0;
                            long end = range.To ?? fileLength - 1;
                            long contentLength = end - start + 1;
                            context.Response.StatusCode = (int)HttpStatusCode.PartialContent;
                            context.Response.Headers.ContentRange =
                                new ContentRangeHeaderValue(start, end, fileLength).ToString();
                            context.Response.Headers.AcceptRanges = "bytes";
                            context.Response.ContentLength = contentLength;

                            await using FileStream stream = File.OpenRead(fullFilePath);
                            stream.Seek(start, SeekOrigin.Begin);
                            await stream.CopyToAsync(context.Response.Body, (int)contentLength);
                        }
                        catch
                        {
                            await context.Response.SendFileAsync(fullFilePath);
                        }
                    else
                        await context.Response.SendFileAsync(fullFilePath);

                    return;
                }
            }

            await _next(context);
        }
        catch (Exception)
        {
            await _next(context);
        }
    }

    public static void AddPath(Ulid requestPath, string physicalPath)
    {
        Providers[requestPath] = new PhysicalFileProvider(physicalPath);
    }

    public static void RemovePath(Ulid requestPath)
    {
        Providers.TryRemove(requestPath, out _);
    }
}
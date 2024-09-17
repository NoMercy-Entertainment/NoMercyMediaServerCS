using HeyRed.ImageSharp.Heif.Formats.Avif;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.NmSystem;
using Serilog.Events;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace NoMercy.Helpers;

public class ImageConvertArguments
{
    [JsonProperty("width")] public int? Width { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("quality")] public int Quality { get; set; } = 100;
    
    public IImageFormat Format
    {
        get
        {
            IImageFormat result;
            try
            {
                result = Images.Parse(Type ?? "png");
            }
            catch (Exception e)
            {
                result = Images.Parse("png");
                Logger.Error(e, LogEventLevel.Error);
            }

            return result;
        }
    }
    
    [FromQuery(Name = "aspect_ratio")]
    [JsonProperty("aspect_ratio")]
    public double? AspectRatio { get; set; }
}

public static class Images
{
    static Images()
    {
        Configuration.Default.ImageFormatsManager.AddImageFormat(AvifFormat.Instance);
        Configuration.Default.ImageFormatsManager.SetEncoder(AvifFormat.Instance, new PngEncoder());
    }
    
    private static byte[] ImageToByteArray(Image<Rgba32> image, ImageConvertArguments arguments)
    {
        using MemoryStream memoryStream = new();
        image.Save(memoryStream, arguments.Format); 
        return memoryStream.ToArray();
    }
    
    public static (byte[] magickImage, string mimeType) ResizeMagickNet(string image, ImageConvertArguments arguments)
    {
        using Image<Rgba32> inputStream = ReadFileStream(image);
        double aspectRatio = arguments.AspectRatio ?? inputStream.Height / (float)inputStream.Width;

        int width = arguments.Width ?? inputStream.Width;
        int height = (int)(width * aspectRatio);

        inputStream.Mutate(x => x.Resize(width, height));
        return (ImageToByteArray(inputStream,arguments), arguments.Format?.MimeTypes.First() ?? "image/png");
    }

    private static Image<Rgba32> ReadFileStream(string image, int attempts = 0)
    {
        if (!File.Exists(image)) throw new Exception("File not found");

        while (attempts < 5)
            try
            {
                return Image.Load<Rgba32>(image);
            }
            catch
            {
                Thread.Sleep(100);
                return ReadFileStream(image, attempts + 1);
            }

        throw new Exception("Failed to read image");
    }

    public static IImageFormat Parse(string format)
    {
        IImageFormat imageFormat;
        Configuration.Default.ImageFormatsManager.TryFindFormatByFileExtension("png", out imageFormat!);
        
        if (string.IsNullOrEmpty(format))
        {
            return imageFormat;
        }
        format = format.ToLowerInvariant();

        if (Configuration.Default.ImageFormatsManager.TryFindFormatByFileExtension(format,
                out IImageFormat? imageFormat2))
        {
                return imageFormat2;
        }
        
        return imageFormat;
    }
}

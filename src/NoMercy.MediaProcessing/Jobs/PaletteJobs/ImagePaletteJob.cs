// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Images;
using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.MediaProcessing.Jobs.PaletteJobs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
public class ImagePaletteJob : AbstractPaletteJob<Image>
{
    public override string QueueName => "image";
    public override int Priority => 2;

    public override async Task Handle()
    {
        await using MediaContext context = new();

        List<Image> images = context.Images
            .Where(x => string.IsNullOrEmpty(x._colorPalette))
            .Where(x => Storage.Select(y => y.FilePath).Contains(x.FilePath))
            .ToList();

        foreach (Image image in images)
        {
            image._colorPalette = await MovieDbImageManager.ColorPalette("image", image.FilePath);
            image.UpdatedAt = DateTime.Now;
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // ignored
        }

        Logger.App($"Image palettes updated: {images.Count}", LogEventLevel.Verbose);
    }
}
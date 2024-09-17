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
public class SimilarPaletteJob : AbstractPaletteJob<Similar>
{
    public override string QueueName => "image";
    public override int Priority => 2;

    public override async Task Handle()
    {
        await using MediaContext? context = new();

        List<Similar> similars = context.Similar
            .Where(x => string.IsNullOrEmpty(x._colorPalette))
            .Where(x => Storage
                .Select(y => y.MovieFromId)
                .Contains(x.MovieFromId))
            .ToList();

        foreach (Similar similar in similars)
            similar._colorPalette = await MovieDbImageManager
                .MultiColorPalette([
                    new BaseImageManager.MultiStringType("poster", similar.Poster),
                    new BaseImageManager.MultiStringType("backdrop", similar.Backdrop)
                ]);

        await context.SaveChangesAsync();

        Logger.App($"Similar palettes updated: {similars.Count}", LogEventLevel.Verbose);
    }
}
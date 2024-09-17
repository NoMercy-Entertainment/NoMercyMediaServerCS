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
public class RecommendationPaletteJob : AbstractPaletteJob<Recommendation>
{
    public override string QueueName => "image";
    public override int Priority => 2;

    public override async Task Handle()
    {
        await using MediaContext? context = new();

        List<Recommendation> recommendations = context.Recommendations
            .Where(x => string.IsNullOrEmpty(x._colorPalette))
            .Where(x => Storage
                .Select(y => y.MovieFromId)
                .Contains(x.MovieFromId))
            .ToList();

        foreach (Recommendation recommendation in recommendations)
            recommendation._colorPalette = await MovieDbImageManager
                .MultiColorPalette([
                    new BaseImageManager.MultiStringType("poster", recommendation.Poster),
                    new BaseImageManager.MultiStringType("backdrop", recommendation.Backdrop)
                ]);

        await context.SaveChangesAsync();

        Logger.App($"Recommendation palettes updated: {recommendations.Count}", LogEventLevel.Verbose);
    }
}
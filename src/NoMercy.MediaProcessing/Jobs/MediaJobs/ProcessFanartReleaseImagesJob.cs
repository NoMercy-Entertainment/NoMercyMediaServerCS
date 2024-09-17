// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using NoMercy.Database;
using NoMercy.MediaProcessing.Images;
using NoMercy.NmSystem;
using NoMercy.Providers.FanArt.Client;
using NoMercy.Providers.FanArt.Models;
using Serilog.Events;

namespace NoMercy.MediaProcessing.Jobs.MediaJobs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
public class ProcessFanartReleaseImagesJob : AbstractFanArtDataJob
{
    public override string QueueName => "image";
    public override int Priority => 5;

    public override async Task Handle()
    {
        await using MediaContext context = new();
        JobDispatcher jobDispatcher = new();

        ImageRepository imageRepository = new(context);
        FanArtImageManager imageManager = new(imageRepository, jobDispatcher);

        try
        {
            using FanArtMusicClient fanArtMusicClient = new();
            FanArtAlbum? fanArt = await fanArtMusicClient.Album(Id1);
            if (fanArt is null) return;

            await imageManager.StoreReleaseImages(fanArt, Id1);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("404")) return;
            Logger.FanArt(e, LogEventLevel.Verbose);
        }
    }
}
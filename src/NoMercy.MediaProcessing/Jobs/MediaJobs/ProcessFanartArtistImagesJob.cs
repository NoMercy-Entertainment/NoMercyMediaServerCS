// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
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
public class ProcessFanartArtistImagesJob : AbstractFanArtDataJob
{
    public override string QueueName => "image";
    public override int Priority => 7;

    public override async Task Handle()
    {
        await using MediaContext context = new();
        JobDispatcher jobDispatcher = new();

        ImageRepository imageRepository = new(context);
        FanArtImageManager imageManager = new(imageRepository, jobDispatcher);
        
        try
        {
            using FanArtMusicClient fanArtMusicClient = new();
            FanArtArtistDetails? fanArt = await fanArtMusicClient.Artist(Id1);
            if (fanArt is null) return;
            
            Artist dbArtist = await context.Artists
                .FirstAsync(a => a.Id == Id1);
            
            await imageManager.StoreReleaseImages(fanArt.ArtistAlbum, Id1, dbArtist);

            try
            {
                Database.Models.Image? artistCover = await imageManager.StoreArtistImages(fanArt, Id1, dbArtist);
                if (artistCover is not null && dbArtist.Cover is not null && dbArtist._colorPalette is not "")
                {
                    dbArtist.Cover = artistCover?.FilePath ?? dbArtist.Cover;

                    dbArtist._colorPalette = artistCover?._colorPalette.Replace("\"image\"", "\"cover\"")
                                             ?? dbArtist._colorPalette;
                    
                    dbArtist.UpdatedAt = DateTime.UtcNow;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        catch (Exception e)
        {
            if (e.Message.Contains("404")) return;
            Logger.FanArt(e, LogEventLevel.Verbose);
        }
    }
}
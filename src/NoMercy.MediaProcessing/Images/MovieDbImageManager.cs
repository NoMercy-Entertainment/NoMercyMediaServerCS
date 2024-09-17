using NoMercy.MediaProcessing.Jobs;
using NoMercy.Providers.TMDB.Client;

namespace NoMercy.MediaProcessing.Images;

public class MovieDbImageManager(
    ImageRepository imageRepository,
    JobDispatcher jobDispatcher
) : IMovieDbImageManager
{
    public async static Task<string> ColorPalette(string type, string? path, bool? download = true)
    {
        return await BaseImageManager.ColorPalette(TmdbImageClient.Download, type, path, download);
    }

    public async static Task<string> MultiColorPalette(IEnumerable<BaseImageManager.MultiStringType> items, bool? download = true)
    {
        return await BaseImageManager.MultiColorPalette(TmdbImageClient.Download, items, download);
    }
}

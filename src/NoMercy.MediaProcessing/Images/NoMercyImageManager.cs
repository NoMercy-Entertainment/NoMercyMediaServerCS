using NoMercy.MediaProcessing.Jobs;
using NoMercy.Providers.NoMercy.Client;

namespace NoMercy.MediaProcessing.Images;

public abstract class NoMercyImageManager(
    ImageRepository imageRepository,
    JobDispatcher jobDispatcher
) : INoMercyImageManager
{
    public async static Task<string> ColorPalette(string type, string? path, bool? download = true)
    {
        return await BaseImageManager.ColorPalette(NoMercyImageClient.Download, type, path, download);
    }

    public async static Task<string> MultiColorPalette(IEnumerable<BaseImageManager.MultiStringType> items, bool? download = true)
    {
        return await BaseImageManager.MultiColorPalette(NoMercyImageClient.Download, items, download);
    }
}
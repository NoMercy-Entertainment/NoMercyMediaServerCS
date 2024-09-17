using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.Episodes;

public interface IEpisodeRepository
{
    public Task StoreEpisodes(IEnumerable<Episode> episodes);
    public Task StoreEpisodeTranslations(IEnumerable<Translation> translations);
    public Task StoreEpisodeImages(IEnumerable<Image> images);
}
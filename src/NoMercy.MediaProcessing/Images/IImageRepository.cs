using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.Images;

public interface IImageRepository
{
    public Task StoreArtistImages(IEnumerable<Image> images, Artist dbArtist);
}
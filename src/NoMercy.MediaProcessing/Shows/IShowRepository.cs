using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Common;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.MediaProcessing.Shows;

public interface IShowRepository
{
    public Task AddAsync(Tv show);
    public Task LinkToLibrary(Library library, Tv show);
    public Task StoreAlternativeTitles(IEnumerable<AlternativeTitle> alternativeTitles);
    public Task StoreTranslations(IEnumerable<Translation> translations);
    public Task StoreContentRatings(IEnumerable<CertificationTv> certifications);
    public Task StoreSimilar(IEnumerable<Similar> similar);
    public Task StoreRecommendations(IEnumerable<Recommendation> recommendations);
    public Task StoreVideos(IEnumerable<Media> videos);
    public Task StoreImages(IEnumerable<Image> images);
    public Task StoreKeywords(IEnumerable<Keyword> keywords);
    public Task LinkKeywordsToTv(IEnumerable<KeywordTv> keywordTvs);
    public Task StoreGenres(IEnumerable<GenreTv> genreTvs);

    public Task StoreWatchProviders();
    public Task StoreNetworks();
    public Task StoreCompanies();

    IEnumerable<CertificationTv> GetCertificationTvs(TmdbTvShowAppends show,
        IEnumerable<CertificationCriteria> certificationCriteria);

    public string GetMediaType(TmdbTvShowAppends show);
}
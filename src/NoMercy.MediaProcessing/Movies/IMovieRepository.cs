using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Common;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.MediaProcessing.Movies;

public interface IMovieRepository
{
    public Task Add(Movie movie);
    public Task LinkToLibrary(Library library, Movie movie);
    public Task StoreAlternativeTitles(IEnumerable<AlternativeTitle> alternativeTitles);
    public Task StoreTranslations(IEnumerable<Translation> translations);
    public Task StoreContentRatings(IEnumerable<CertificationMovie> certifications);
    public Task StoreSimilar(IEnumerable<Similar> similar);
    public Task StoreRecommendations(IEnumerable<Recommendation> recommendations);
    public Task StoreVideos(IEnumerable<Media> videos);
    public Task StoreImages(IEnumerable<Image> images);
    public Task StoreKeywords(IEnumerable<Keyword> keywords);
    public Task LinkKeywordsToMovie(IEnumerable<KeywordMovie> keywordMovies);
    public Task StoreGenres(IEnumerable<GenreMovie> genreMovies);

    public Task StoreWatchProviders();
    public Task StoreNetworks();
    public Task StoreCompanies();

    IEnumerable<CertificationMovie> GetCertificationMovies(TmdbMovieAppends movie,
        IEnumerable<CertificationCriteria> certificationCriteria);
}
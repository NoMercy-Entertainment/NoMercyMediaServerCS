using NoMercy.NmSystem;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Providers.TMDB.Client.Mocks;

public class MovieResponseMocks
{
    public TmdbMovieAppends? MockMovieAppendsResponse()
    {
        return MovieAppendsResponse.Value().FromJson<TmdbMovieAppends>();
    }
}
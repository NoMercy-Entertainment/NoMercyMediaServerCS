using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Jobs;
using NoMercy.MediaProcessing.Jobs.MediaJobs;

namespace NoMercy.Data.Repositories;

public class MovieRepository(MediaContext context) : IMovieRepository
{
    public Task<Movie?> GetMovieAsync(Guid userId, int id, string language)
    {
        return context.Movies.AsNoTracking()
            .Where(movie => movie.Id == id)
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            .Include(movie => movie.MovieUser
                .Where(movieUser => movieUser.UserId == userId)
            )
            .Include(movie => movie.Cast)
            .ThenInclude(castMovie => castMovie.Person)
            .Include(movie => movie.Cast)
            .ThenInclude(castMovie => castMovie.Role)
            .Include(movie => movie.Crew)
            .ThenInclude(crewMovie => crewMovie.Person)
            .Include(movie => movie.Crew)
            .ThenInclude(crewMovie => crewMovie.Job)
            .Include(movie => movie.Library)
            .ThenInclude(library => library.LibraryUsers)
            .Include(movie => movie.Media)
            .Include(movie => movie.AlternativeTitles)
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == language))
            .Include(movie => movie.Images
                .Where(image =>
                    (image.Type == "logo" && image.Iso6391 == "en")
                    || ((image.Type == "backdrop" || image.Type == "poster") &&
                        (image.Iso6391 == "en" || image.Iso6391 == null))
                )
                .OrderByDescending(image => image.VoteAverage)
            )
            .Include(movie => movie.CertificationMovies)
            .ThenInclude(certificationMovie => certificationMovie.Certification)
            .Include(movie => movie.GenreMovies)
            .ThenInclude(genreMovie => genreMovie.Genre)
            .Include(movie => movie.KeywordMovies)
            .ThenInclude(keywordMovie => keywordMovie.Keyword)
            .Include(movie => movie.RecommendationFrom)
            .Include(movie => movie.SimilarFrom)
            .Include(movie => movie.VideoFiles)
            .ThenInclude(file => file.UserData.Where(
                userData => userData.UserId == userId))
            .FirstOrDefaultAsync();
    }

    public Task<bool> GetMovieAvailableAsync(Guid userId, int id)
    {
        return context.Movies.AsNoTracking()
            .Where(movie => movie.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            .Where(movie => movie.Id == id)
            .Include(movie => movie.VideoFiles)
            .AnyAsync();
    }

    public IEnumerable<Movie> GetMoviePlaylistAsync(Guid userId, int id, string language)
    {
        return context.Movies.AsNoTracking()
            .Where(movie => movie.Id == id)
            .Where(movie => movie.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            .Include(movie => movie.Media
                .Where(media => media.Type == "video"))
            .Include(movie => movie.Images
                .Where(image => image.Type == "logo"))
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == language))
            .Include(movie => movie.VideoFiles)
            .ThenInclude(file => file.UserData.Where(
                userData => userData.UserId == userId));
    }

    public async Task<bool> LikeMovieAsync(int id, Guid userId, bool like)
    {
        try
        {
            MovieUser? movieUser = await context.MovieUser
                .FirstOrDefaultAsync(mu => mu.MovieId == id && mu.UserId == userId);

            if (like)
            {
                await context.MovieUser.Upsert(new MovieUser(id, userId))
                    .On(m => new { m.MovieId, m.UserId })
                    .WhenMatched(m => new MovieUser
                    {
                        MovieId = m.MovieId,
                        UserId = m.UserId
                    })
                    .RunAsync();
            }
            else if (movieUser != null)
            {
                context.MovieUser.Remove(movieUser);
                await context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task AddMovieAsync(int id)
    {
        Library? tvLibrary = await context.Libraries
            .Where(f => f.Type == "movie")
            .FirstOrDefaultAsync();

        if (tvLibrary == null) return;

        JobDispatcher jobDispatcher = new();
        jobDispatcher.DispatchJob<AddMovieJob>(id, tvLibrary);
    }
}
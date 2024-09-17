using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface IGenreRepository
{
    Task<Genre> GetGenreAsync(Guid userId, int id, string language, int take, int page);
    IQueryable<Genre> GetGenresAsync(Guid userId, string language, int take, int page);
}
using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface ITvShowRepository
{
    Task<Tv?> GetTvAsync(Guid userId, int id, string language);
    Task<bool> GetTvAvailableAsync(Guid userId, int id);
    Task<Tv?> GetTvPlaylistAsync(Guid userId, int id, string language);
    Task<bool> LikeTvAsync(int id, Guid userId, bool like);

    Task AddTvShowAsync(int id);
}
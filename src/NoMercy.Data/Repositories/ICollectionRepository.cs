using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface ICollectionRepository
{
    Task<List<Collection>> GetCollectionsAsync(Guid userId, string language, int take, int page);
    Task<Collection?> GetCollectionAsync(Guid userId, int id, string? language, string country);
    Task<Collection?> GetAvailableCollectionAsync(Guid userId, int id);
    Task<Collection?> GetWatchCollectionAsync(Guid userId, int id, string language, string country);
    Task<bool> LikeCollectionAsync(int id, Guid userId, bool like);
}
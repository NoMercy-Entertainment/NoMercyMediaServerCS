using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface IFolderRepository
{
    Task<Folder?> GetFolderByIdAsync(Ulid folderId);
    Task<Folder?> GetFolderByPathAsync(string requestPath);
    Task AddFolderAsync(Folder folder);
    Task AddFolderLibraryAsync(FolderLibrary folderLibrary);
    Task AddFolderLibraryAsync(FolderLibrary[] folderLibraries);
    Task UpdateFolderAsync(Folder folder);
    Task DeleteFolderAsync(Folder folder);
    Task<List<Folder>> GetFoldersByLibraryIdAsync(Ulid libraryId);
    Task<List<Folder>> GetFoldersByLibraryIdAsync(FolderLibraryDto[] requestFolderLibrary);
}
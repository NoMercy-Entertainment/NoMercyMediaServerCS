using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface ILibraryRepository
{
    IQueryable<Library> GetLibrariesAsync(Guid userId);
    Task<Library> GetLibraryByIdAsync(Ulid libraryId, Guid userId, string language, int take, int page);
    IQueryable<Movie> GetLibraryMoviesAsync(Guid userId, Ulid libraryId, string language, int take, int page);
    IQueryable<Tv> GetLibraryShowsAsync(Guid userId, Ulid libraryId, string language, int take, int page);

    IOrderedQueryable<Library> GetDashboardLibrariesAsync(Guid userId);
    Task<Library?> GetLibraryByIdAsync(Ulid id);
    Task AddLibraryAsync(Library library, Guid userId);
    Task UpdateLibraryAsync(Library library);
    Task DeleteLibraryAsync(Library library);
    Task<List<Library>> GetAllLibrariesAsync();

    Task AddEncoderProfileFolderAsync(EncoderProfileFolder encoderProfileFolder);
    Task AddEncoderProfileFolderAsync(List<EncoderProfileFolder> encoderProfileFolders);
    Task AddEncoderProfileFolderAsync(EncoderProfileFolder[] encoderProfileFolders);
    Task AddLanguageLibraryAsync(LanguageLibrary[] languageLibraries);
    List<FolderDto> GetFoldersAsync();
}
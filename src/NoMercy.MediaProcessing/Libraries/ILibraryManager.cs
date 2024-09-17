namespace NoMercy.MediaProcessing.Libraries;

public interface ILibraryManager
{
    Task ProcessLibrary(Ulid id);
}
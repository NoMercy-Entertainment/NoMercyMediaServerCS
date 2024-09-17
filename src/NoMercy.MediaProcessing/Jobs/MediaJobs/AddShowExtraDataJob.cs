// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using NoMercy.Database;
using NoMercy.MediaProcessing.Files;
using NoMercy.MediaProcessing.People;
using NoMercy.MediaProcessing.Shows;
using NoMercy.Networking;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.MediaProcessing.Jobs.MediaJobs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
public class AddShowExtraDataJob : AbstractMediaExraDataJob<TmdbTvShowAppends>
{
    public override string QueueName => "queue";
    public override int Priority => 1;

    public override async Task Handle()
    {
        await using MediaContext context = new();
        JobDispatcher jobDispatcher = new();
        FileRepository fileRepository = new(context);

        ShowRepository showRepository = new(context);
        ShowManager showManager = new(showRepository, jobDispatcher);

        PersonRepository personRepository = new(context);
        PersonManager personManager = new(personRepository, jobDispatcher);

        await personManager.Store(Storage);
        
        await showManager.StoreImages(Storage);
        await showManager.StoreSimilar(Storage);
        await showManager.StoreRecommendations(Storage);
        await showManager.StoreAlternativeTitles(Storage);
        await showManager.StoreWatchProviders(Storage);
        await showManager.StoreVideos(Storage);
        await showManager.StoreNetworks(Storage);
        await showManager.StoreCompanies(Storage);
        await showManager.StoreKeywords(Storage);

        Networking.Networking.SendToAll("RefreshLibrary", "socket", new RefreshLibraryDto
        {
            QueryKey = ["tv", Storage.Id.ToString()]
        });
    }
}
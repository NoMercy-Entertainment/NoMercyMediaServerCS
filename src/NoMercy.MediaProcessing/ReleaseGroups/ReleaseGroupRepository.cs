using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.ReleaseGroups;

public class ReleaseGroupRepository(MediaContext context) : IReleaseGroupRepository
{
    public Task Store(ReleaseGroup releaseGroup)
    {
        return context.ReleaseGroups.Upsert(releaseGroup)
            .On(e => new { e.Id })
            .WhenMatched((s, i) => new ReleaseGroup
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Year = i.Year,
                LibraryId = i.LibraryId,
                UpdatedAt = i.UpdatedAt
            })
            .RunAsync();
    }
}
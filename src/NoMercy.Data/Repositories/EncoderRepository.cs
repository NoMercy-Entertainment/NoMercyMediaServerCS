using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public class EncoderRepository(MediaContext context) : IEncoderRepository
{
    public Task<List<EncoderProfile>> GetEncoderProfilesAsync()
    {
        return context.EncoderProfiles
            .ToListAsync();
    }

    public Task<EncoderProfile?> GetEncoderProfileByIdAsync(Ulid id)
    {
        return context.EncoderProfiles
            .FirstOrDefaultAsync(profile => profile.Id == id);
    }

    public Task AddEncoderProfileAsync(EncoderProfile profile)
    {
        return context.EncoderProfiles.Upsert(profile)
            .On(l => new { l.Id })
            .WhenMatched((ls, li) => new EncoderProfile
            {
                Id = li.Id,
                Name = li.Name,
                Container = li.Container,
                Param = li.Param,
                UpdatedAt = li.UpdatedAt
            })
            .RunAsync();
    }

    public Task DeleteEncoderProfileAsync(EncoderProfile profile)
    {
        context.EncoderProfiles
            .Remove(profile);

        return context.SaveChangesAsync();
    }

    public Task<int> GetEncoderProfileCountAsync()
    {
        return context.EncoderProfiles
            .CountAsync();
    }
}
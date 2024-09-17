using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.Recordings;

public class RecordingRepository(MediaContext context) : IRecordingRepository
{
    public Task Store(Track recording, bool update = false)
    {
        return context.Tracks.Upsert(recording)
            .On(e => new { e.Id })
            .WhenMatched((ts, ti) => new Track
            {
                UpdatedAt = DateTime.UtcNow,
                Id = ti.Id,
                Name = ti.Name,
                DiscNumber = ti.DiscNumber,
                TrackNumber = ti.TrackNumber,
                Date = ti.Date,

                Folder = update ? ts.Folder : ti.Folder,
                FolderId = update ? ts.FolderId : ti.FolderId,
                HostFolder = update ? ts.HostFolder : ti.HostFolder,
                Duration = update ? ts.Duration : ti.Duration,
                Filename = update ? ts.Filename : ti.Filename,
                Quality = update ? ts.Quality : ti.Quality
            })
            .RunAsync();
    }

    public Task LinkToRelease(AlbumTrack trackRelease)
    {
        return context.AlbumTrack.Upsert(trackRelease)
            .On(e => new { e.AlbumId, e.TrackId })
            .WhenMatched((s, i) => new AlbumTrack
            {
                AlbumId = i.AlbumId,
                TrackId = i.TrackId
            })
            .RunAsync();
    }

    public Task LinkToArtist(ArtistTrack insert)
    {
        return context.ArtistTrack.Upsert(insert)
            .On(e => new { e.ArtistId, e.TrackId })
            .WhenMatched((s, i) => new ArtistTrack
            {
                ArtistId = i.ArtistId,
                TrackId = i.TrackId
            })
            .RunAsync();
    }
}
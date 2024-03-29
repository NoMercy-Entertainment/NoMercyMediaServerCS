﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(TrackId))]
    public class LibraryTrack
    {
        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }
        
        [JsonProperty("track_id")] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        public LibraryTrack()
        {
        }

        public LibraryTrack(Ulid libraryId, Guid trackId)
        {
            LibraryId = libraryId;
            TrackId = trackId;
        }
    }
}
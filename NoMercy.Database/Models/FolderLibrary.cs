﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(FolderId), nameof(LibraryId))]
    public class FolderLibrary
    {
        [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
        public virtual Folder Folder { get; set; }
        
        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public FolderLibrary(Ulid folderId, Ulid libraryId)
        {
            FolderId = folderId;
            LibraryId = libraryId;
        }

        public FolderLibrary()
        {
        }
    }
}
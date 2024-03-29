﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(EncoderProfileId), nameof(FolderId))]
    public class EncoderProfileFolder
    {
        [JsonProperty("encoder_profile_id")] public Ulid EncoderProfileId { get; set; }
        public virtual EncoderProfile EncoderProfile { get; set; }

        [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
        public virtual Folder Folder { get; set; }

        public EncoderProfileFolder()
        {
        }

        public EncoderProfileFolder(Ulid encoderProfileId, Ulid libraryId)
        {
            EncoderProfileId = encoderProfileId;
            FolderId = libraryId;
        }
    }
}
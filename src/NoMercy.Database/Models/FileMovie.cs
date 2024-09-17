#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(FileId), nameof(MovieId))]
[Index(nameof(FileId))]
[Index(nameof(MovieId))]
public class FileMovie(Ulid fileId, int movieId) {
    [JsonProperty("file_id")] public Ulid FileId { get; set; } = fileId;
    public File File { get; set; }

    [JsonProperty("movie_id")] public int MovieId { get; set; } = movieId;
    public Movie Movie { get; set; }

}
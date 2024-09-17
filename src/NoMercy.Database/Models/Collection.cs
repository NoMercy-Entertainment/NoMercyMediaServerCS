using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Collection : ColorPalettes
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("title_sort")] public string? TitleSort { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("parts")] public int Parts { get; set; }

    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    public Library Library { get; set; }

    [JsonProperty("collection_movies")]
    public ICollection<CollectionMovie> CollectionMovies { get; set; } = new HashSet<CollectionMovie>();

    [JsonProperty("translations")] public ICollection<Translation> Translations { get; set; }

    [JsonProperty("images")] public ICollection<Image> Images { get; set; }

    [JsonProperty("collection_user")] public ICollection<CollectionUser> CollectionUser { get; set; }

    [JsonProperty("user_data")] public ICollection<UserData> UserData { get; set; }

    public Collection()
    {
    }

    // public Collection(TmdbCollectionAppends tmdbCollection, Ulid libraryId)
    // {
    //     Id = tmdbCollection.Id;
    //     Title = tmdbCollection.Name;
    //     TitleSort = tmdbCollection.Name.TitleSort(tmdbCollection.Parts.MinBy(movie => movie.ReleaseDate)?.ReleaseDate);
    //     Backdrop = tmdbCollection.BackdropPath;
    //     Poster = tmdbCollection.PosterPath;
    //     Overview = tmdbCollection.Overview;
    //     Parts = tmdbCollection.Parts.Length;
    //     LibraryId = libraryId;
    // }
}
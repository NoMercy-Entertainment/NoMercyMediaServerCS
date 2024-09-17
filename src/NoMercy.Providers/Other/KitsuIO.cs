using Newtonsoft.Json;
using NoMercy.Networking;
using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.Providers.Other;

public static class KitsuIo
{
    public static async Task<bool> IsAnime(string title, int year)
    {
        bool isAnime = false;

        HttpClient client = new();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(ApiInfo.UserAgent);
        client.BaseAddress = new Uri("https://kitsu.io/api/edge/");

        HttpResponseMessage response = await client.GetAsync($"anime?filter[text]={title}&filter[year]={year}");
        string content = await response.Content.ReadAsStringAsync();

        try
        {
            KitsuAnime? anime = content.FromJson<KitsuAnime>();

            foreach (Data data in anime?.Data ?? [])
                if (data.Attributes.Titles.En?.Equals(title, StringComparison.CurrentCultureIgnoreCase) == true)
                    isAnime = true;
                else if (data.Attributes.Titles.EnJp?.Equals(title, StringComparison.CurrentCultureIgnoreCase) == true)
                    isAnime = true;
                else if (data.Attributes.Titles.JaJp?.Equals(title, StringComparison.CurrentCultureIgnoreCase) == true)
                    isAnime = true;
                else if (data.Attributes.Titles.ThTh?.Equals(title, StringComparison.CurrentCultureIgnoreCase) == true)
                    isAnime = true;
                else if (data.Attributes.AbbreviatedTitles.Any(abbreviatedTitle =>
                             abbreviatedTitle.Equals(title, StringComparison.CurrentCultureIgnoreCase)))
                    isAnime = true;
        }
        catch (Exception e)
        {
            Logger.AniDb(e, LogEventLevel.Fatal);
        }

        return isAnime;
    }
}

public class KitsuAnime
{
    [JsonProperty("data")] public Data[] Data { get; set; } = [];
    [JsonProperty("meta")] public KitsuIoMeta Meta { get; set; } = new();
    [JsonProperty("links")] public KitsuIoLinks Links { get; set; } = new();
}

public class Data
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("links")] public Links Links { get; set; } = new();
    [JsonProperty("attributes")] public Attributes Attributes { get; set; } = new();
    [JsonProperty("relationships")] public Dictionary<string, Relationship> Relationships { get; set; } = new();
}

public class Attributes
{
    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("slug")] public string Slug { get; set; } = string.Empty;
    [JsonProperty("synopsis")] public string Synopsis { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("coverImageTopOffset")] public int CoverImageTopOffset { get; set; }
    [JsonProperty("titles")] public Titles Titles { get; set; } = new();
    [JsonProperty("canonicalTitle")] public string CanonicalTitle { get; set; } = string.Empty;
    [JsonProperty("abbreviatedTitles")] public string[] AbbreviatedTitles { get; set; } = [];
    [JsonProperty("averageRating")] public string? AverageRating { get; set; }
    [JsonProperty("ratingFrequencies")] public Dictionary<string, int> RatingFrequencies { get; set; } = new();
    [JsonProperty("userCount")] public int UserCount { get; set; }
    [JsonProperty("favoritesCount")] public int? FavoritesCount { get; set; }
    [JsonProperty("startDate")] public DateTime? StartDate { get; set; }
    [JsonProperty("endDate")] public DateTime? EndDate { get; set; }
    [JsonProperty("nextRelease")] public object? NextRelease { get; set; }
    [JsonProperty("popularityRank")] public int? PopularityRank { get; set; }
    [JsonProperty("ratingRank")] public int? RatingRank { get; set; }
    [JsonProperty("ageRating")] public string? AgeRating { get; set; }
    [JsonProperty("ageRatingGuide")] public string AgeRatingGuide { get; set; } = string.Empty;
    [JsonProperty("subtype")] public string Subtype { get; set; } = string.Empty;
    [JsonProperty("status")] public string Status { get; set; } = string.Empty;
    [JsonProperty("tba")] public string? Tba { get; set; }
    [JsonProperty("posterImage")] public PosterImage? PosterImage { get; set; }
    [JsonProperty("coverImage")] public CoverImage? CoverImage { get; set; }
    [JsonProperty("episodeCount")] public int? EpisodeCount { get; set; }
    [JsonProperty("episodeLength")] public int? EpisodeLength { get; set; }
    [JsonProperty("totalLength")] public int? TotalLength { get; set; }
    [JsonProperty("youtubeVideoId")] public string? YoutubeVideoId { get; set; }
    [JsonProperty("showType")] public string? ShowType { get; set; }
    [JsonProperty("nsfw")] public bool Nsfw { get; set; }
}

public class CoverImage
{
    [JsonProperty("tiny")] public Uri? Tiny { get; set; }
    [JsonProperty("large")] public Uri? Large { get; set; }
    [JsonProperty("small")] public Uri? Small { get; set; }
    [JsonProperty("original")] public Uri? Original { get; set; }
    [JsonProperty("meta")] public CoverImageMeta? Meta { get; set; }
}

public class CoverImageMeta
{
    [JsonProperty("dimensions")] public Dimensions? Dimensions { get; set; }
}

public class Dimensions
{
    [JsonProperty("tiny")] public Large? Tiny { get; set; }
    [JsonProperty("large?")] public Large? Large { get; set; }
    [JsonProperty("small")] public Large? Small { get; set; }

    [JsonProperty("medium")] public Large? Medium { get; set; }
}

public class Large
{
    [JsonProperty("width")] public int? Width { get; set; }
    [JsonProperty("height")] public int? Height { get; set; }
}

public class PosterImage
{
    [JsonProperty("tiny")] public Uri? Tiny { get; set; }
    [JsonProperty("large")] public Uri? Large { get; set; }
    [JsonProperty("small")] public Uri? Small { get; set; }
    [JsonProperty("medium")] public Uri? Medium { get; set; }
    [JsonProperty("original")] public Uri? Original { get; set; }
    [JsonProperty("meta")] public CoverImageMeta? Meta { get; set; }
}

public class Titles
{
    [JsonProperty("en")] public string? En { get; set; }
    [JsonProperty("en_jp")] public string? EnJp { get; set; }
    [JsonProperty("ja_jp")] public string? JaJp { get; set; }
    [JsonProperty("th_th")] public string? ThTh { get; set; }
}

public class Links
{
    [JsonProperty("self")] public Uri Self { get; set; } = default!;
}

public class Relationship
{
    [JsonProperty("links")] public RelationshipLinks Links { get; set; } = new();
}

public class RelationshipLinks
{
    [JsonProperty("self")] public Uri Self { get; set; } = default!;
    [JsonProperty("related")] public Uri Related { get; set; } = default!;
}

public class KitsuIoLinks
{
    [JsonProperty("first")] public Uri First { get; set; } = default!;
    [JsonProperty("last")] public Uri Last { get; set; } = default!;
}

public class KitsuIoMeta
{
    [JsonProperty("count")] public int Count { get; set; }
}
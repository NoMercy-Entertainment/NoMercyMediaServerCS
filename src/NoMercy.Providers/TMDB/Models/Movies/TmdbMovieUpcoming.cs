using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieUpcoming : TmdbPaginatedResponse<TmdbMovie>
{
    [JsonProperty("dates")] public TmdbUpcomingMovieDates MovieDates { get; set; } = new();
}

public class TmdbUpcomingMovieDates
{
    [JsonProperty("maximum")] public DateTime? Maximum { get; set; }
    [JsonProperty("minimum")] public DateTime? Minimum { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Networking;

public partial class ApiInfo
{
    public static readonly string ApplicationVersion = Environment.Version.ToString();
    public static readonly string ApplicationName = "NoMercy MediaServer";

    public static string MakeMkvKey { get; set; } = string.Empty;
    public static string TmdbKey { get; set; } = string.Empty;
    public static string OmdbKey { get; set; } = string.Empty;
    public static string FanArtKey { get; set; } = string.Empty;
    public static string RottenTomatoes { get; set; } = string.Empty;
    public static string AcousticIdKey { get; set; } = string.Empty;
    public static string TadbKey { get; set; } = string.Empty;
    public static string TmdbToken { get; set; } = string.Empty;
    public static string TvdbKey { get; set; } = string.Empty;
    public static string MusixmatchKey { get; set; } = string.Empty;
    public static string JwplayerKey { get; set; } = string.Empty;
    public static Downloads BinaryList { get; private set; } = new();
    public static string[] Colors { get; private set; } = [];
    public static string Quote { get; private set; } = string.Empty;
    public const string UserAgent = "NoMercy MediaServer/0.1.0 ( admin@nomercy.tv )";

    public static async Task RequestInfo()
    {
        HttpClient client = new();
        client.Timeout = TimeSpan.FromSeconds(15);
        client.BaseAddress = new Uri(Config.ApiBaseUrl);
        HttpResponseMessage response = await client.GetAsync("v1/info");
        string? content = await response.Content.ReadAsStringAsync();

        if (content == null) throw new Exception("Failed to get server info");

        try
        {
            ApiInfo? data = JsonConvert.DeserializeObject<ApiInfo>(content);
            if (data == null) throw new Exception("Failed to deserialize server info");

            Quote = data.Data.Quote;
            Colors = data.Data.Colors;

            BinaryList = data.Data.Downloads;

            MakeMkvKey = data.Data.Keys.MakeMkvKey;
            TmdbKey = data.Data.Keys.TmdbKey;
            OmdbKey = data.Data.Keys.OmdbKey;
            FanArtKey = data.Data.Keys.FanArtKey;
            RottenTomatoes = data.Data.Keys.RottenTomatoes;
            AcousticIdKey = data.Data.Keys.AcousticId;
            TadbKey = data.Data.Keys.TadbKey;
            TmdbToken = data.Data.Keys.TmdbToken;
            TvdbKey = data.Data.Keys.TvdbKey;
            MusixmatchKey = data.Data.Keys.MusixmatchKey;
            JwplayerKey = data.Data.Keys.JwplayerKey;
        }
        catch (Exception e)
        {
            // Logger.Setup(e, LogEventLevel.Error);
            throw;
        }
    }
}

#region Types

public partial class ApiInfo
{
    [JsonProperty("status")] public string Status { get; set; } = string.Empty;
    [JsonProperty("data")] public Data Data { get; set; } = new();
}

public class Data
{
    [JsonProperty("state")] public string State { get; set; } = string.Empty;
    [JsonProperty("version")] public string Version { get; set; } = string.Empty;
    [JsonProperty("copyright")] public string Copyright { get; set; } = string.Empty;
    [JsonProperty("licence")] public string Licence { get; set; } = string.Empty;
    [JsonProperty("contact")] public Contact Contact { get; set; } = new();
    [JsonProperty("git")] public Uri? Git { get; set; }
    [JsonProperty("keys")] public Keys Keys { get; set; } = new();
    [JsonProperty("quote")] public string Quote { get; set; } = string.Empty;
    [JsonProperty("colors")] public string[] Colors { get; set; } = [];
    [JsonProperty("downloads")] public Downloads Downloads { get; set; } = new();
}

public class Contact
{
    [JsonProperty("homepage")] public string Homepage { get; set; } = string.Empty;
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("email")] public string Email { get; set; } = string.Empty;
    [JsonProperty("dmca")] public string Dmca { get; set; } = string.Empty;
    [JsonProperty("languages")] public string Languages { get; set; } = string.Empty;
    [JsonProperty("socials")] public Socials Socials { get; set; } = new();
}

public class Socials
{
    [JsonProperty("twitch")] public Uri? Twitch { get; set; }
    [JsonProperty("youtube")] public Uri? Youtube { get; set; }
    [JsonProperty("twitter")] public Uri? Twitter { get; set; }
    [JsonProperty("discord")] public string Discord { get; set; } = string.Empty;
}

public class Downloads
{
    [JsonProperty("windows")] public List<Download> Windows { get; set; } = [];
    [JsonProperty("linux")] public List<Download> Linux { get; set; } = [];
    [JsonProperty("mac")] public List<Download> Mac { get; set; } = [];
}

public class Download
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("url")] public Uri? Url { get; set; }
    [JsonProperty("filter")] public string Filter { get; set; } = string.Empty;
    [JsonProperty("last_updated")] public DateTime LastUpdated { get; set; }
}

public class Keys
{
    [JsonProperty("makemkv_key")] public string MakeMkvKey { get; set; } = string.Empty;
    [JsonProperty("tmdb_key")] public string TmdbKey { get; set; } = string.Empty;
    [JsonProperty("omdb_key")] public string OmdbKey { get; set; } = string.Empty;
    [JsonProperty("fanart_key")] public string FanArtKey { get; set; } = string.Empty;
    [JsonProperty("rotten_tomatoes")] public string RottenTomatoes { get; set; } = string.Empty;
    [JsonProperty("acoustic_id")] public string AcousticId { get; set; } = string.Empty;
    [JsonProperty("tadb_key")] public string TadbKey { get; set; } = string.Empty;
    [JsonProperty("tmdb_token")] public string TmdbToken { get; set; } = string.Empty;
    [JsonProperty("tvdb_key")] public string TvdbKey { get; set; } = string.Empty;
    [JsonProperty("musixmatch_key")] public string MusixmatchKey { get; set; } = string.Empty;
    [JsonProperty("jwplayer_key")] public string JwplayerKey { get; set; } = string.Empty;
}

#endregion
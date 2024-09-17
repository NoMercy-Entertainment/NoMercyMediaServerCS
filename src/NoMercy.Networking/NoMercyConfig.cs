namespace NoMercy.Networking;

public static class NoMercyConfig
{
    public static string AuthBaseUrl { get; set; } = "https://auth.nomercy.tv/realms/NoMercyTV/";
    public static string TokenClientSecret { get; set; } = "1lHWBazSTHfBpuIzjAI6xnNjmwUnryai";
    public const string TokenClientId = "nomercy-server";

    public static string AppBaseUrl { get; set; } = "https://app.nomercy.tv/";
    public static string ApiBaseUrl { get; set; } = "https://api.nomercy.tv/";
    public static string ApiServerBaseUrl { get; set; } = $"{ApiBaseUrl}v1/server/";

    public static int InternalServerPort { get; set; } = 7626;
    public static int ExternalServerPort { get; set; } = 7626;

    public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    
    public static string AppPath => Path.Combine(AppDataPath, "NoMercy_C#");
    public static string ConfigPath => Path.Combine(AppPath, "config");
    public static string TokenFile => Path.Combine(ConfigPath, "token.json");
    public static string ConfigFile => Path.Combine(ConfigPath, "config.json");
    
    public static bool IsDev { get; set; }
}
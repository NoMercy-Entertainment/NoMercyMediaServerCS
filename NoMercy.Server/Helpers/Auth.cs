using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace NoMercy.Server.Helpers;

public static class Auth
{
    private static readonly string BaseUrl = "https://auth-dev2.nomercy.tv/realms/NoMercyTV";
    private static readonly string AuthBaseUrl = $"{BaseUrl}";
    private static readonly string TokenUrl = $"{AuthBaseUrl}/protocol/openid-connect/token";
    
    private static string? PublicKey { set; get; }
    private static string? TokenClientId { set; get; } = "nomercy-server";
    private static string? TokenClientSecret { set; get; } = "1lHWBazSTHfBpuIzjAI6xnNjmwUnryai";

    private static string? RefreshToken { set; get; }
    public static string? AccessToken { private set; get; }
    private static int? ExpiresIn { get; set; }

    private static int? NotBefore { get; set; }

    private static JwtSecurityToken? _jwtSecurityToken;

    private static IWebHost? TempServer { get; set; }

    public static void Init()
    {
        if(!File.Exists(AppFiles.TokenFile))
        { 
            File.WriteAllText(AppFiles.TokenFile, "{}");
        }
        
        GetAuthKeys();
        
        AccessToken = GetAccessToken();
        RefreshToken = GetRefreshToken();
        ExpiresIn = GetTokenExpiration();
        NotBefore = GetTokenNotBefore();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        _jwtSecurityToken = tokenHandler.ReadJwtToken(AccessToken);
        
        int expiresInDays = _jwtSecurityToken.ValidTo.AddDays(-5).Subtract(DateTime.UtcNow).Days;
        
        bool expired = NotBefore == null && ExpiresIn != null && expiresInDays >= 0;
        
        if(RefreshToken != null && !expired)
            GetTokenByRefreshGrand();
        else
            GetTokenByBrowser();
        
        if (AccessToken == null || RefreshToken == null || ExpiresIn == null)
            throw new Exception("Failed to get tokens");
        
    }
    
    private static void GetTokenByBrowser()
    {
        string redirectUri =HttpUtility.UrlEncode($"http://localhost:" + Networking.InternalServerPort + "/sso-callback");
        string url = "https://auth-dev2.nomercy.tv/realms/NoMercyTV/protocol/openid-connect/auth?redirect_uri=" +
                     redirectUri + "&client_id=nomercy-server&response_type=code&scope=openid%20offline_access%20email%20profile";

        TempServer = Networking.TempServer();
        TempServer.StartAsync().Wait();
        
        OpenBrowser(url);
        
        CheckToken();
    }

    private static void CheckToken()
    {
        Task.Run(async () =>
        {
            await Task.Delay(1000);

            if (AccessToken == null || RefreshToken == null || ExpiresIn == null)
                CheckToken();
            else
                TempServer?.StopAsync().Wait();
        }).Wait();
    }

    private static void SetTokens(string response)
    {
        dynamic data = JsonConvert.DeserializeObject(response) 
                       ?? throw new Exception("Failed to deserialize JSON");
        
        if (data.access_token == null || data.refresh_token == null || data.expires_in == null)
        {
            throw new Exception("Failed to get authentication tokens");
        }

        var tmp = File.OpenWrite(AppFiles.TokenFile);
        tmp.SetLength(0);
        tmp.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data, Formatting.Indented)));
        tmp.Close();

        Console.WriteLine("Tokens refreshed");
        
        AccessToken = data.access_token;
        RefreshToken = data.refresh_token;
        ExpiresIn = data.expires_in;
        NotBefore = data["not-before-policy"];
    }
    private static dynamic GetTokenData()
    {
        return JsonConvert.DeserializeObject(File.ReadAllText(AppFiles.TokenFile))
               ?? throw new Exception("Failed to deserialize JSON");
    }

    private static string? GetAccessToken()
    {
        dynamic data = GetTokenData();

        return data.access_token;
    }

    private static string? GetRefreshToken() {
        dynamic data = GetTokenData();
        return data.refresh_token;
    }

    private static int? GetTokenExpiration() {
        dynamic data = GetTokenData();
        return data.expires_in;
    }
    
    private static int? GetTokenNotBefore()
    {
        dynamic data = GetTokenData();
        return data["not-before-policy"];
    }

    private static void GetAuthKeys()
    {
        Console.WriteLine("Getting auth keys");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string response = client.GetStringAsync(AuthBaseUrl).Result;
        
        dynamic data = JsonConvert.DeserializeObject(response) 
                       ?? throw new Exception("Failed to deserialize JSON");
        
        PublicKey = data.public_key;
    }
    
    private static void GetTokenByPasswordGrant(string username, string password)
    {
        if (TokenClientId == null || TokenClientSecret == null)
            throw new Exception("Auth keys not initialized");
        
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var body = new List<KeyValuePair<string, string>>()
        {
            new ("grant_type", "password"),
            new ("client_id", TokenClientId),
            new ("client_secret", TokenClientSecret),
            new ("username", username),
            new ("password", password),
        };
        
        string response = client.PostAsync(TokenUrl, new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;
        
        SetTokens(response);
    }
    private static void GetTokenByRefreshGrand()
    {
        
        if (TokenClientId == null || TokenClientSecret == null || RefreshToken == null || _jwtSecurityToken == null)
            throw new Exception("Auth keys not initialized");

        int expiresInDays = _jwtSecurityToken.ValidTo.AddDays(-5).Subtract(DateTime.UtcNow).Days;
        if(expiresInDays >= 0)
        {
            Console.WriteLine("Token is still valid for {0} day{1}", expiresInDays, expiresInDays == 1 ? "" : "s");
            return;
        };
        
        Console.WriteLine("Refreshing token");
        
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var body = new List<KeyValuePair<string, string>>()
        {
            new ("grant_type", "refresh_token"),
            new ("client_id", TokenClientId),
            new ("client_secret", TokenClientSecret),
            new ("refresh_token", RefreshToken),
            new ("scope", "openid offline_access email profile"),
        };
        
        string response = client.PostAsync(TokenUrl,new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;
        
        SetTokens(response);
    }
    public static void GetTokenByAuthorizationCode(string code)
    {
        Console.WriteLine("Getting token by authorization code");
        if (TokenClientId == null || TokenClientSecret == null)
            throw new Exception("Auth keys not initialized");
        
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var body = new List<KeyValuePair<string, string>>()
        {
            new ("grant_type", "authorization_code"),
            new ("client_id", TokenClientId),
            new ("client_secret", TokenClientSecret),
            new ("scope", "openid offline_access email profile"),
            new ("redirect_uri", $"http://localhost:{Networking.InternalServerPort}/sso-callback"),
            new ("code", code),
        };
        
        string response = client.PostAsync(TokenUrl, new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;
        
        SetTokens(response);
    }
    
    private static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);  // Works ok on linux
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url); // Not tested
        }
        else
        {
            throw new Exception("Unsupported OS");
        }
    }

}
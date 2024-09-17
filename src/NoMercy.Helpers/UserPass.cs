namespace NoMercy.Helpers;
public class UserPass(string username, string password, string apiKey) {
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    public string? ApiKey { get; set; } = apiKey;

}
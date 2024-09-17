using System.Security.Cryptography;
using System.Text;
using NoMercy.NmSystem;

namespace NoMercy.Providers.Helpers;

public static class CacheController
{
    public static string GenerateFileName(string url)
    {
        return CreateMd5(url);
    }

    private static string CreateMd5(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    public static bool Read<T>(string url, out T? value) where T : class?
    {
        string fullname = Path.Combine(AppFiles.ApiCachePath, GenerateFileName(url));
        lock (fullname)
        {
            if (System.IO.File.Exists(fullname) == false)
            {
                value = default;
                return false;
            }

            T? data;
            try
            {
                string d = System.IO.File.ReadAllTextAsync(fullname).Result;
                data = JsonHelper.FromJson<T>(d);
            }
            catch (Exception)
            {
                value = default;
                return false;
            }

            if (data == null)
            {
                value = default;
                return true;
            }

            if (data is { } item)
            {
                value = item;
                return true;
            }

            value = default;
            return false;
        }
    }

    public static async Task Write(string url, string data, int retry = 0)
    {
        string fullname = Path.Combine(AppFiles.ApiCachePath, GenerateFileName(url));

        try
        {
            await System.IO.File.WriteAllTextAsync(fullname, data);
        }
        catch (Exception)
        {
            if (retry >= 10)
            {
                Logger.App($"CacheController: Failed to write {fullname}");
                throw;
            }

            await Write(url, data, retry + 1);
        }
    }
}
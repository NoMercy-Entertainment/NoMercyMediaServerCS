﻿using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Providers.CoverArt.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Configuration = AcoustID.Configuration;
using Image = SixLabors.ImageSharp.Image;

namespace NoMercy.Providers.FanArt.Client;

public class FanArtImageClient : FanArtBaseClient
{
    public FanArtImageClient()
    {
        Configuration.ClientKey = ApiInfo.AcousticIdKey;
    }

    public Task<CoverArtCovers?> Cover(bool priority = false)
    {
        Dictionary<string, string?> queryParams = new()
        {
            //
        };

        return Get<CoverArtCovers>("release/" + Id, queryParams, priority);
    }

    public async static Task<Image<Rgba32>?> Download(Uri url, bool? download = true)
    {
        string filePath = Path.Combine(AppFiles.MusicImagesPath, Path.GetFileName(url.LocalPath));

        if (System.IO.File.Exists(filePath)) return Image.Load<Rgba32>(filePath);

        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("User-Agent", ApiInfo.UserAgent);
        httpClient.DefaultRequestHeaders.Add("Accept", "image/*");
        httpClient.BaseAddress = new Uri("https://assets.fanart.tv");

        HttpResponseMessage response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        Stream stream = await response.Content.ReadAsStreamAsync();

        if (download is false) return Image.Load<Rgba32>(stream);

        if (!System.IO.File.Exists(filePath))
            await System.IO.File.WriteAllBytesAsync(filePath, await response.Content.ReadAsByteArrayAsync());

        return Image.Load<Rgba32>(stream);
    }
}
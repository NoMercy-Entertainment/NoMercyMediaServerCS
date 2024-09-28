using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NoMercy.MediaProcessing.Images;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Encoder.Format.Rules;
using NoMercy.MediaProcessing.Jobs;
using NoMercy.MediaProcessing.Jobs.MediaJobs;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Providers.MusicBrainz.Client;
using NoMercy.Providers.NoMercy.Data;
using NoMercy.Providers.NoMercy.Models.Specials;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Certifications;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;
using Serilog.Events;
using Certification = NoMercy.Database.Models.Certification;
using File = System.IO.File;
using Special = NoMercy.Database.Models.Special;

namespace NoMercy.Data.Logic;

public class Seed : IDisposable, IAsyncDisposable
{
    private static TmdbConfigClient TmdbConfigClient { get; set; } = new();
    private static TmdbMovieClient TmdbMovieClient { get; set; } = new();
    private static TmdbTvClient TmdbTvClient { get; set; } = new();
    private static readonly MediaContext MediaContext = new();
    private static readonly QueueContext QueueContext = new();
    private static Folder[] _folders = [];
    private static User[] _users = [];
    private static bool ShouldSeedMarvel { get; set; }

    public static async Task Init(bool shouldSeedMarvel)
    { 
        ShouldSeedMarvel = shouldSeedMarvel;
        await CreateDatabase();
        await SeedDatabase();
    }

    private static async Task CreateDatabase()
    {
        try
        {
            await MediaContext.Database.EnsureCreatedAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e.Message, LogEventLevel.Error);
        }

        try
        {
            await QueueContext.Database.EnsureCreatedAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e.Message, LogEventLevel.Error);
        }
    }

    private static async Task SeedDatabase()
    {
        try
        {
            await AddGenres();
            await AddCertifications();
            await AddLanguages();
            await AddCountries();
            await AddMusicGenres();
            await AddFolderRoots();
            await AddEncoderProfiles();
            await AddLibraries();
            await Users();

            if (ShouldSeedMarvel)
            {
                Thread thread = new(() => _ = AddSpecial());
                thread.Start();
            }
        }
        catch (Exception e)
        {
            Logger.Setup(e.Message, LogEventLevel.Error);
        }
    }

    private static async Task AddEncoderProfiles()
    {
        bool hasEncoderProfiles = await MediaContext.EncoderProfiles.AnyAsync();
        // if (hasEncoderProfiles) return;
        
        Logger.Setup("Adding Encoder Profiles");

        EncoderProfile[] encoderProfiles;
        // if (File.Exists(AppFiles.EncoderProfilesSeedFile))
        //     encoderProfiles = File.ReadAllTextAsync(AppFiles.EncoderProfilesSeedFile).Result
        //         .FromJson<EncoderProfile[]>() ?? [];
        // else
            encoderProfiles =
            [
                new EncoderProfile
                {
                    Id = Ulid.Parse("01HQ6298ZSZYKJT83WDWTPG4G8"),
                    Name = "Marvel 4k",
                    Container = VideoContainers.Hls,
                    EncoderProfileFolder = [
                        new EncoderProfileFolder
                        {
                            FolderId = Ulid.Parse("01J8T6PB9JDE801599F7YGPGE8"),
                            EncoderProfileId = Ulid.Parse("01HQ6298ZSZYKJT83WDWTPG4G8"),
                        },
                        new EncoderProfileFolder
                        {
                            FolderId = Ulid.Parse("01J8T6PDZYCR8JQ8EVQDGCFK8W"),
                            EncoderProfileId = Ulid.Parse("01HQ6298ZSZYKJT83WDWTPG4G8"),
                        }
                    ],
                    VideoProfiles = [
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._1080p.Width,
                            Crf = 20,
                            SegmentName = ":type:_:framesize:/:type:_:framesize:",
                            PlaylistName = ":type:_:framesize:/:type:_:framesize:",
                            ColorSpace = ColorSpaces.Yuv444p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            // // Opts = ["no-scenecut"],
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        },
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._1080p.Width,
                            Crf = 20,
                            SegmentName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            PlaylistName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            ColorSpace = ColorSpaces.Yuv420p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        },
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._4k.Width,
                            Crf = 20,
                            SegmentName = ":type:_:framesize:/:type:_:framesize:",
                            PlaylistName = ":type:_:framesize:/:type:_:framesize:",
                            ColorSpace = ColorSpaces.Yuv444p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        },
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._4k.Width,
                            Crf = 20,
                            SegmentName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            PlaylistName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            ColorSpace = ColorSpaces.Yuv420p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        },
                    ],
                    AudioProfiles = [
                        new IAudioProfile
                        {
                            Codec = AudioCodecs.Aac.Value,
                            Channels = 2,
                            SegmentName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            PlaylistName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ]
                        },
                        new IAudioProfile
                        {
                            Codec = AudioCodecs.TrueHd.Value,
                            SegmentName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            PlaylistName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ]
                        }
                    ],
                    SubtitleProfiles = [
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Webvtt.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        },
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Ass.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        }
                    ]
                },
                // new EncoderProfile
                // {
                //     Id = Ulid.Parse("01HQ629JAYQDEQAH0GW3ZHGW8Z"),
                //     Name = "1080p high",
                //     Container = VideoContainers.Hls,
                //     EncoderProfileFolder = [
                //         new EncoderProfileFolder
                //         {
                //             FolderId = Ulid.Parse("01J8T6PB9JDE801599F7YGPGE8"),
                //         },
                //         new EncoderProfileFolder
                //         {
                //             FolderId = Ulid.Parse("01J8T6PB9JDE801599F7YGPGE8"),
                //         }
                //     ],
                //     VideoProfiles = [
                //         new IVideoProfile
                //         {
                //             Codec = VideoCodecs.H264Nvenc.Value,
                //             Width = FrameSizes._1080p.Width,
                //             Crf = 20,
                //             SegmentName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                //             PlaylistName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                //             ColorSpace = ColorSpaces.Yuv420p,
                //             Preset = VideoPresets.Fast,
                //             Tune = VideoTunes.Hq,
                //             Keyint = 48,
                //             CustomArguments = [
                //                 new ValueTuple<string, string>()
                //                 {
                //                     Item1 = "-x264opts",
                //                     Item2 = "no-scenecut"
                //                 }
                //             ]
                //         }
                //     ],
                //     AudioProfiles = [
                //         new IAudioProfile
                //         {
                //             Codec = AudioCodecs.Aac.Value,
                //             Channels = 2,
                //             SegmentName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                //             PlaylistName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                //             AllowedLanguages = [
                //                 Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                //                 Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                //             ]
                //         },
                //     ],
                //     SubtitleProfiles = [
                //         new ISubtitleProfile
                //         {
                //             Codec = SubtitleCodecs.Webvtt.Value,
                //             AllowedLanguages = [
                //                 Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                //                 Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                //             ],
                //             PlaylistName = "subtitles/:filename:.:language:.:variant:"
                //         }
                //     ]
                // },
                new EncoderProfile
                {
                    Id = Ulid.Parse("01HQ629SJ32FTV2Q46NX3H1CK9"),
                    Name = "1080p regular",
                    Container = VideoContainers.Hls,
                    EncoderProfileFolder = [
                        new EncoderProfileFolder
                        {
                            FolderId = Ulid.Parse("01HQ5W78J5ADPV6K0SBZRBGWE3"),
                            EncoderProfileId = Ulid.Parse("01HQ629SJ32FTV2Q46NX3H1CK9"),
                        },
                        new EncoderProfileFolder
                        {
                            FolderId = Ulid.Parse("01HQ5W67GRBPHJKNAZMDYKMVXA"),
                            EncoderProfileId = Ulid.Parse("01HQ629SJ32FTV2Q46NX3H1CK9"),
                        }
                    ],
                    VideoProfiles = [
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._1080p.Width,
                            Crf = 23,
                            SegmentName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            PlaylistName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            ColorSpace = ColorSpaces.Yuv420p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        }
                    ],
                    AudioProfiles = [
                        new IAudioProfile
                        {
                            Codec = AudioCodecs.Aac.Value,
                            Channels = 2,
                            SegmentName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            PlaylistName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ]
                        },
                    ],
                    SubtitleProfiles = [
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Webvtt.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        },
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Ass.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        }
                    ]
                },
                new EncoderProfile
                {
                    Id = Ulid.Parse("01HR360AKTW47XC6ZQ2V9DF024"),
                    Name = "1080p low",
                    Container = VideoContainers.Hls,
                    EncoderProfileFolder = [
                        new EncoderProfileFolder
                        {
                            FolderId = Ulid.Parse("01HQ5W4Y1ZHYZKS87P0AG24ERE"),
                            EncoderProfileId = Ulid.Parse("01HR360AKTW47XC6ZQ2V9DF024"),
                        },
                    ],
                    VideoProfiles = [
                        new IVideoProfile
                        {
                            Codec = VideoCodecs.H264Nvenc.Value,
                            Width = FrameSizes._1080p.Width,
                            Crf = 25,
                            SegmentName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            PlaylistName = ":type:_:framesize:_SDR/:type:_:framesize:_SDR",
                            ColorSpace = ColorSpaces.Yuv420p,
                            Preset = VideoPresets.Fast,
                            Tune = VideoTunes.Hq,
                            Keyint = 48,
                            CustomArguments = [
                                new ValueTuple<string, string>()
                                {
                                    Item1 = "-x264opts",
                                    Item2 = "no-scenecut"
                                }
                            ]
                        }
                    ],
                    AudioProfiles = [
                        new IAudioProfile
                        {
                            Codec = AudioCodecs.Aac.Value,
                            Channels = 2,
                            SegmentName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            PlaylistName = ":type:_:language:_:codec:/:type:_:language:_:codec:",
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ]
                        },
                    ],
                    SubtitleProfiles = [
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Webvtt.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        },
                        new ISubtitleProfile
                        {
                            Codec = SubtitleCodecs.Ass.Value,
                            AllowedLanguages = [
                                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita,
                                Languages.Spa,  Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                            ],
                            PlaylistName = "subtitles/:filename:.:language:.:variant:"
                        }
                    ]
                }
            ];

        await File.WriteAllTextAsync(AppFiles.EncoderProfilesSeedFile, encoderProfiles.ToJson());

        await MediaContext.EncoderProfiles.UpsertRange(encoderProfiles)
            .On(v => new { v.Id })
            .WhenMatched((vs, vi) => new EncoderProfile
            {
                Id = vi.Id,
                Name = vi.Name,
                Container = vi.Container,
                Param = vi.Param,
                _videoProfiles = vi._videoProfiles,
                _audioProfiles = vi._audioProfiles,
                _subtitleProfiles = vi._subtitleProfiles,
                UpdatedAt = vi.UpdatedAt,
            })
            .RunAsync();

        List<EncoderProfileFolder> encoderProfileFolders = [];
        foreach (var encoderProfile in encoderProfiles)
        {
            encoderProfileFolders.AddRange(encoderProfile.EncoderProfileFolder.ToList()
                .Select(encoderProfileFolder => new EncoderProfileFolder
                {
                    EncoderProfileId = encoderProfile.Id,
                    FolderId = encoderProfileFolder.FolderId
                }));
        }

        await MediaContext.EncoderProfileFolder
            .UpsertRange(encoderProfileFolders)
            .On(v => new { v.FolderId, v.EncoderProfileId })
            .WhenMatched((vs, vi) => new EncoderProfileFolder
            {
                FolderId = vi.FolderId,
                EncoderProfileId = vi.EncoderProfileId
            })
            .RunAsync();
    }

    private static async Task Users()
    {
        Logger.Setup("Adding Users");
        
        HttpClient client = new();
        client.BaseAddress = new Uri(Config.ApiServerBaseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", ApiInfo.UserAgent);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

        IDictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("server_id", Info.DeviceId.ToString());

        string newUrl = QueryHelpers.AddQueryString("users", query);

        HttpResponseMessage response = await client.GetAsync(newUrl);
        string? content = await response.Content.ReadAsStringAsync();

        if (content == null) throw new Exception("Failed to get Server info");

        ServerUserDto[] serverUsers = content.FromJson<ServerUserDto[]>() ?? [];
        
        Logger.Setup($"Found {serverUsers.Length} users");

        _users = serverUsers.ToList()
            .ConvertAll<User>(serverUser => new User
            {
                Id = serverUser.UserId,
                Email = serverUser.Email,
                Name = serverUser.Name,
                Allowed = serverUser.Enabled,
                Manage = serverUser.Enabled,
                AudioTranscoding = serverUser.Enabled,
                NoTranscoding = serverUser.Enabled,
                VideoTranscoding = serverUser.Enabled,
                Owner = serverUser.IsOwner,
                UpdatedAt = DateTime.Now
            })
            .ToArray();

        await MediaContext.Users
            .UpsertRange(_users)
            .On(v => new { v.Id })
            .WhenMatched((us, ui) => new User
            {
                Id = ui.Id,
                Email = ui.Email,
                Name = ui.Name,
                Allowed = ui.Allowed,
                Manage = ui.Manage,
                AudioTranscoding = ui.AudioTranscoding,
                NoTranscoding = ui.NoTranscoding,
                VideoTranscoding = ui.VideoTranscoding,
                Owner = ui.Owner,
                UpdatedAt = ui.UpdatedAt
            })
            .RunAsync();

        if (!File.Exists(AppFiles.LibrariesSeedFile)) return;

        Library[] libraries = File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)
            .Result.FromJson<Library[]>() ?? [];

        List<LibraryUser> libraryUsers = [];

        foreach (User user in _users.ToList())
        foreach (Library library in libraries.ToList())
            libraryUsers.Add(new LibraryUser
            {
                LibraryId = library.Id,
                UserId = user.Id
            });

        await MediaContext.LibraryUser
            .UpsertRange(libraryUsers)
            .On(v => new { v.LibraryId, v.UserId })
            .WhenMatched((lus, lui) => new LibraryUser
            {
                LibraryId = lui.LibraryId,
                UserId = lui.UserId
            })
            .RunAsync();
    }

    private static async Task AddGenres()
    {
        bool hasGenres = await MediaContext.Genres.AnyAsync();
        if (hasGenres) return;
        
        Logger.Setup("Adding Genres");

        List<Genre> genres = [];

        genres.AddRange(
            (await TmdbMovieClient.Genres())?
            .Genres.ToList()
            .ConvertAll<Genre>(genre => new Genre
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToArray() ?? []
        );

        genres.AddRange(
            (await TmdbTvClient.Genres())?
            .Genres.ToList()
            .ConvertAll<Genre>(genre => new Genre
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToArray() ?? []
        );

        await MediaContext.Genres.UpsertRange(genres)
            .On(v => new { v.Id })
            .WhenMatched(v => new Genre
            {
                Id = v.Id,
                Name = v.Name
            })
            .RunAsync();
    }

    private static async Task AddCertifications()
    {
        bool hasCertifications = await MediaContext.Certifications.AnyAsync();
        if (hasCertifications) return;
        
        Logger.Setup("Adding Certifications");

        List<Certification> certifications = [];

        foreach ((string key, TmdbMovieCertification[] value) in (await TmdbMovieClient.Certifications())
                 ?.Certifications ?? [])
        foreach (TmdbMovieCertification certification in value)
            certifications.Add(new Certification
            {
                Iso31661 = key,
                Rating = certification.Rating,
                Meaning = certification.Meaning,
                Order = certification.Order
            });

        foreach ((string key, TmdbTvShowCertification[] value) in (await TmdbTvClient.Certifications())
                 ?.Certifications ?? [])
        foreach (TmdbTvShowCertification certification in value)
            certifications.Add(new Certification
            {
                Iso31661 = key,
                Rating = certification.Rating,
                Meaning = certification.Meaning,
                Order = certification.Order
            });

        await MediaContext.Certifications.UpsertRange(certifications)
            .On(v => new { v.Iso31661, v.Rating })
            .WhenMatched(v => new Certification
            {
                Iso31661 = v.Iso31661,
                Rating = v.Rating,
                Meaning = v.Meaning,
                Order = v.Order
            })
            .RunAsync();
    }

    private static async Task AddLanguages()
    {
        bool hasLanguages = await MediaContext.Languages.AnyAsync();
        if (hasLanguages) return;
        
        Logger.Setup("Adding Languages");

        Language[] languages = (await TmdbConfigClient.Languages())?.ToList()
            .ConvertAll<Language>(language => new Language
            {
                Iso6391 = language.Iso6391,
                EnglishName = language.EnglishName,
                Name = language.Name
            }).ToArray() ?? [];

        await MediaContext.Languages.UpsertRange(languages)
            .On(v => new { v.Iso6391 })
            .WhenMatched(v => new Language
            {
                Iso6391 = v.Iso6391,
                Name = v.Name,
                EnglishName = v.EnglishName
            })
            .RunAsync();
    }

    private static async Task AddCountries()
    {
        bool hasCountries = await MediaContext.Countries.AnyAsync();
        if (hasCountries) return;
        
        Logger.Setup("Adding Countries");

        Country[] countries = (await TmdbConfigClient.Countries())?.ToList()
            .ConvertAll<Country>(country => new Country
            {
                Iso31661 = country.Iso31661,
                EnglishName = country.EnglishName,
                NativeName = country.NativeName
            }).ToArray() ?? [];

        await MediaContext.Countries.UpsertRange(countries)
            .On(v => new { v.Iso31661 })
            .WhenMatched(v => new Country
            {
                Iso31661 = v.Iso31661,
                NativeName = v.NativeName,
                EnglishName = v.EnglishName
            })
            .RunAsync();
    }

    private static async Task AddMusicGenres()
    {
        bool hasMusicGenres = await MediaContext.MusicGenres.AnyAsync();
        if (hasMusicGenres) return;
        
        Logger.Setup("Adding Music Genres");

        MusicBrainzGenreClient musicBrainzGenreClient = new();

        MusicGenre[] genres = (await musicBrainzGenreClient.All()).ToList()
            .ConvertAll<MusicGenre>(genre => new MusicGenre
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToArray();

        await MediaContext.MusicGenres.UpsertRange(genres)
            .On(v => new { v.Id })
            .WhenMatched(v => new MusicGenre
            {
                Id = v.Id,
                Name = v.Name
            })
            .RunAsync();

        await Task.CompletedTask;
    }

    private static async Task AddFolderRoots()
    {
        try
        {
            if (!File.Exists(AppFiles.FolderRootsSeedFile)) return;
            
            Logger.Setup("Adding Folder Roots");

            _folders = File.ReadAllTextAsync(AppFiles.FolderRootsSeedFile)
                .Result.FromJson<Folder[]>() ?? [];

            await MediaContext.Folders.UpsertRange(_folders)
                .On(v => new { v.Id })
                .WhenMatched((vs, vi) => new Folder()
                {
                    Id = vi.Id,
                    Path = vi.Path
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e, LogEventLevel.Error);
        }
    }

    private static async Task AddLibraries()
    {
        try
        {
            if (!File.Exists(AppFiles.LibrariesSeedFile)) return;
            
            Logger.Setup("Adding Libraries");

            LibrarySeedDto[] librarySeed = File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)
                .Result.FromJson<LibrarySeedDto[]>() ?? [];

            List<Library> libraries = librarySeed.Select(librarySeedDto => new Library()
            {
                Id = librarySeedDto.Id,
                AutoRefreshInterval = librarySeedDto.AutoRefreshInterval,
                ChapterImages = librarySeedDto.ChapterImages,
                ExtractChapters = librarySeedDto.ExtractChapters,
                ExtractChaptersDuring = librarySeedDto.ExtractChaptersDuring,
                Image = librarySeedDto.Image,
                PerfectSubtitleMatch = librarySeedDto.PerfectSubtitleMatch,
                Realtime = librarySeedDto.Realtime,
                SpecialSeasonName = librarySeedDto.SpecialSeasonName,
                Title = librarySeedDto.Title,
                Type = librarySeedDto.Type,
                Order = librarySeedDto.Order
            }).ToList();

            await MediaContext.Libraries.UpsertRange(libraries)
                .On(v => new { v.Id })
                .WhenMatched((vs, vi) => new Library()
                {
                    Id = vi.Id,
                    AutoRefreshInterval = vi.AutoRefreshInterval,
                    ChapterImages = vi.ChapterImages,
                    ExtractChapters = vi.ExtractChapters,
                    ExtractChaptersDuring = vi.ExtractChaptersDuring,
                    Image = vi.Image,
                    PerfectSubtitleMatch = vi.PerfectSubtitleMatch,
                    Realtime = vi.Realtime,
                    SpecialSeasonName = vi.SpecialSeasonName,
                    Title = vi.Title,
                    Type = vi.Type,
                    Order = vi.Order
                })
                .RunAsync();

            List<FolderLibrary> libraryFolders = [];

            foreach (LibrarySeedDto library in librarySeed.ToList())
            foreach (FolderDto folder in library.Folders.ToList())
                libraryFolders.Add(new FolderLibrary(folder.Id, library.Id));

            await MediaContext.FolderLibrary
                .UpsertRange(libraryFolders)
                .On(v => new { v.FolderId, v.LibraryId })
                .WhenMatched((vs, vi) => new FolderLibrary()
                {
                    FolderId = vi.FolderId,
                    LibraryId = vi.LibraryId
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e.Message, LogEventLevel.Error);
        }
    }

    private static async Task AddSpecial()
    {
        Logger.Setup("Adding Special");

        try
        {
            await using MediaContext context = new();
            Library movieLibrary = await context.Libraries
                .Where(f => f.Type == "movie")
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstAsync();

            Library tvLibrary = await context.Libraries
                .Where(f => f.Type == "tv")
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstAsync();

            Special special = new()
            {
                Id = Mcu.Special.Id,
                Title = Mcu.Special.Title,
                Backdrop = Mcu.Special.Backdrop,
                Poster = Mcu.Special.Poster,
                Logo = Mcu.Special.Logo,
                Description = Mcu.Special.Description,
                Creator = Mcu.Special.Creator,
                _colorPalette = await NoMercyImageManager
                    .MultiColorPalette(new[]
                    {
                        new BaseImageManager.MultiStringType("poster", Mcu.Special.Poster),
                        new BaseImageManager.MultiStringType("backdrop", Mcu.Special.Backdrop)
                    })
            };

            await context.Specials
                .Upsert(special)
                .On(v => new { v.Id })
                .WhenMatched((si, su) => new Special()
                {
                    Id = su.Id,
                    Title = su.Title,
                    Backdrop = su.Backdrop,
                    Poster = su.Poster,
                    Logo = su.Logo,
                    Description = su.Description,
                    Creator = su.Creator,
                    _colorPalette = su._colorPalette
                })
                .RunAsync();

            TmdbSearchClient client = new();
            List<int> tvIds = [];
            List<int> movieIds = [];
            List<SpecialItem> specialItems = [];
            
            JobDispatcher jobDispatcher = new();

            foreach (CollectionItem item in Mcu.McuItems)
            {
                Logger.Setup($"Searching for {item.title} ({item.year})");
                switch (item.type)
                {
                    case "movie":
                    {
                        TmdbPaginatedResponse<TmdbMovie>? result =
                            await client.Movie(item.title, item.year.ToString());
                        TmdbMovie? movie = result?.Results.FirstOrDefault(
                            r => r.Title.ToLower().Contains("making of") == false);

                        if (movie is null) continue;
                        if (movieIds.Contains(movie.Id)) continue;

                        movieIds.Add(movie.Id);

                        try
                        {
                            bool exists = context.Movies.Any(x => x.Id == movie.Id);
                            if (!exists)
                            {
                                AddMovieJob j = new AddMovieJob
                                {
                                    Id = movie.Id,
                                    LibraryId = movieLibrary.Id
                                };
                                j.Handle().Wait();
                                // jobDispatcher.DispatchJob<AddMovieJob>(movie.Id, movieLibrary);
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Setup(e, LogEventLevel.Fatal);
                        }

                        break;
                    }
                    case "tv":
                    {
                        TmdbPaginatedResponse<TmdbTvShow>? result =
                            await client.TvShow(item.title, item.year.ToString());
                        TmdbTvShow? tv = result?.Results.FirstOrDefault(r =>
                            r.Name.Contains("making of", StringComparison.InvariantCultureIgnoreCase) == false);

                        if (tv is null) continue;
                        if (tvIds.Contains(tv.Id)) continue;

                        tvIds.Add(tv.Id);

                        try
                        {
                            bool exists = context.Tvs.Any(x => x.Id == tv.Id);
                            if (!exists)
                            {
                                AddShowJob j = new AddShowJob
                                {
                                    Id = tv.Id,
                                    LibraryId = tvLibrary.Id
                                };
                                j.Handle().Wait();
                                // jobDispatcher.DispatchJob<AddMovieJob>(movie.Id, movieLibrary);
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Setup(e, LogEventLevel.Fatal);
                        }

                        break;
                    }
                }
            }

            foreach (CollectionItem item in Mcu.McuItems)
            {
                Logger.Setup($"Searching for {item.title} ({item.year})");
                switch (item.type)
                {
                    case "movie":
                    {
                        TmdbPaginatedResponse<TmdbMovie>? result =
                            await client.Movie(item.title, item.year.ToString());
                        TmdbMovie? movie = result?.Results.FirstOrDefault(r =>
                            r.Title.Contains("making of", StringComparison.InvariantCultureIgnoreCase) == false);
                        if (movie is null) continue;

                        specialItems.Add(new SpecialItem
                        {
                            SpecialId = special.Id,
                            MovieId = movie.Id,
                            Order = specialItems.Count
                        });

                        break;
                    }
                    case "tv":
                    {
                        TmdbPaginatedResponse<TmdbTvShow>? result =
                            await client.TvShow(item.title, item.year.ToString());
                        TmdbTvShow? tv = result?.Results.FirstOrDefault(r =>
                            r.Name.Contains("making of", StringComparison.InvariantCultureIgnoreCase) == false);
                        if (tv is null) continue;

                        if (item.episodes.Length == 0)
                            item.episodes = context.Episodes
                                .Where(x => x.TvId == tv.Id)
                                .Where(x => x.SeasonNumber == item.seasons.First())
                                .Select(x => x.EpisodeNumber)
                                .ToArray();

                        foreach (int episodeNumber in item.episodes)
                        {
                            Episode? episode = context.Episodes
                                .FirstOrDefault(x =>
                                    x.TvId == tv.Id
                                    && x.SeasonNumber == item.seasons.First()
                                    && x.EpisodeNumber == episodeNumber);

                            if (episode is null) continue;

                            specialItems.Add(new SpecialItem
                            {
                                SpecialId = special.Id,
                                EpisodeId = episode.Id,
                                Order = specialItems.Count
                            });
                        }

                        break;
                    }
                }
            }

            Logger.Setup($"Upsetting {specialItems.Count} SpecialItems");

            IEnumerable<SpecialItem> movies = specialItems
                .Where(s => s.MovieId is not null);

            foreach (SpecialItem movie in movies)
            {
                try
                {
                    await context.SpecialItems.Upsert(movie)
                        .On(x => new { x.SpecialId, x.MovieId })
                        .WhenMatched((old, @new) => new SpecialItem
                        {
                            SpecialId = @new.SpecialId,
                            MovieId = @new.MovieId,
                            Order = @new.Order
                        })
                        .RunAsync();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
            IEnumerable<SpecialItem> episodes = specialItems
                .Where(s => s.EpisodeId is not null);

            foreach (SpecialItem episode in episodes)
            {
                try
                {
                    await context.SpecialItems.Upsert(episode)
                        .On(x => new { x.SpecialId, x.EpisodeId })
                        .WhenMatched((old, @new) => new SpecialItem
                        {
                            SpecialId = @new.SpecialId,
                            EpisodeId = @new.EpisodeId,
                            Order = @new.Order
                        })
                        .RunAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        catch (Exception e)
        {
            Logger.Setup(e, LogEventLevel.Error);
            throw;
        }
    }

    public void Dispose()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
        return ValueTask.CompletedTask;
    }
}
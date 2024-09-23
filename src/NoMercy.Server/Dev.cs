using System.Net;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Encoder;
using NoMercy.Encoder.Core;
using NoMercy.Encoder.Format.Audio;
using NoMercy.Encoder.Format.Container;
using NoMercy.Encoder.Format.Image;
using NoMercy.Encoder.Format.Rules;
using NoMercy.Encoder.Format.Video;
using NoMercy.MediaProcessing.Jobs;
using NoMercy.MediaProcessing.Jobs.MediaJobs;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Providers.OpenSubtitles.Client;
using NoMercy.Providers.OpenSubtitles.Models;
using Vtt = NoMercy.Encoder.Format.Subtitle.Vtt;

namespace NoMercy.Server;

public class Dev
{
    public static async void Run()
    {
        // OpenSubtitlesClient client = new();
        // OpenSubtitlesClient subtitlesClient = await client.Login();
        // SubtitleSearchResponse? x = await subtitlesClient.SearchSubtitles("Black Panther Wakanda Forever (2022)", "dut");
        // Logger.OpenSubs(x);
        
        // MediaContext mediaContext = new();
        // JobDispatcher jobDispatcher = new();

        // Library? movieLibrary = await mediaContext.Libraries
        //     .Where(f => f.Type == "movie")
        //     .FirstOrDefaultAsync();

        // Library? tvLibrary = await mediaContext.Libraries
        //     .Where(f => f.Type == "tv")
        //     .FirstOrDefaultAsync();

        // Library? animeLibrary = await mediaContext.Libraries
        //     .Where(f => f.Type == "anime")
        //     .FirstOrDefaultAsync();

        // int[] tvIds =
        // [
        //     44310, 39340, 1973, 75219, 30980, 90546, 30977, 122587, 75123, 74092, 77778, 64537, 75777, 912, 130237,
        //     65292, 70637, 61259, 72519, 1433, 42942, 74080, 42589, 63088, 45348, 62110, 246, 77529, 41727, 203780,
        //     61695, 70716, 50712, 73223, 12598, 38464, 82728, 99083, 212963, 95, 66840, 65931, 102086, 83962, 138357,
        //     74728, 71024, 93256, 80671, 114410, 66912, 63145, 44006, 58841, 73017, 1615, 37527, 72528, 1404, 76063,
        //     24835, 72517, 70636, 31724, 100567, 58474, 30991, 72503, 45859, 46004, 76122, 13916, 85937, 67026, 86824,
        //     12577, 75208, 1405, 4229, 131927, 73021, 6326, 60810, 720, 42410, 54930, 122226, 31682, 1415, 42671, 196040,
        //     46261, 1434, 196285, 90677, 62273, 61456, 1668, 39379, 615, 72426, 231677, 1399, 63453, 70128, 34793, 76757,
        //     79166, 31731, 60730, 60863, 34749, 1126, 88329, 88987, 80665, 45950, 56998, 110070, 1408, 19716, 1425, 1100,
        //     46298, 232125, 40178, 71007, 82738, 158307, 119335, 46025, 40424, 62745, 68080, 74096, 45799, 42253, 72305,
        //     83054, 46374, 37585, 2345, 70593, 74097, 45998, 2384, 72635, 60831, 66080, 8358, 53715, 69293, 84958, 72026,
        //     86831, 63174, 72636, 72788, 39483, 60732, 85324, 83043, 114695, 102693, 61550, 1403, 69088, 73919, 59427,
        //     66190, 61889, 63181, 133903, 40044, 68716, 62127, 38472, 62126, 111312, 92788, 67466, 72705, 62285, 67178,
        //     69291, 21732, 21730, 46195, 92749, 139287, 81358, 62560, 92782, 72296, 65930, 62565, 2317, 77723, 1428,
        //     4614, 124271, 17610, 61387, 95317, 890, 80350, 62640, 60808, 64710, 29117, 39272, 2632, 69298, 64394, 56570,
        //     64196, 77701, 22254, 1877, 19780, 20177, 60572, 8392, 43865, 62816, 45502, 42510, 74163, 113687, 96316,
        //     60625, 60845, 71790, 90855, 71789, 120089, 78239, 42444, 62742, 28061, 6005, 64434, 114472, 34805, 61664,
        //     1087, 71018, 92783, 66857, 50825, 2190, 26707, 45844, 42509, 78102, 26563, 37680, 117933, 100414, 45782,
        //     78204, 72525, 75975, 114937, 48561, 66077, 68639, 53347, 48866, 40546, 1481, 64671, 1418, 46952, 68018,
        //     15632, 45997, 67676, 81046, 88396, 1996, 69629, 71712, 72844, 62430, 2362, 100088, 33880, 82856, 80670,
        //     5920, 61367, 15621, 50325, 78471, 76719, 83095, 79744, 90802, 62104, 456, 47640, 68590, 64163, 56351, 4429,
        //     34742, 205308, 68101, 127714, 46331, 66926, 44217, 75214, 137, 64414, 85271, 117821, 119051, 91363, 86836,
        //     85844, 45967, 71728, 61663, 80504, 60866, 83105
        // ];

        // int[] movieIds =
        // [
        //     584, 38142, 17979, 15789, 937278, 40073, 1238514, 54272, 21174, 20737, 21661, 21518, 24794, 20755, 149,
        //     1074034, 15653, 13448, 102899, 363088, 640146, 1579, 11633, 45295, 269650, 271924, 265805, 13981, 99861,
        //     299534, 299536, 46842, 339403, 628861, 40260, 9737, 8961, 38700, 573435, 408648, 1016121, 1096342, 10020,
        //     321612, 13313, 601844, 20235, 34075, 702525, 284054, 505642, 497698, 78, 335984, 15049, 417489, 62177,
        //     11932, 400106, 417644, 822119, 271110, 1771, 100402, 299537, 920, 49013, 260514, 46844, 10515, 39860, 53014,
        //     640, 302699, 818119, 198184, 181283, 10585, 11186, 11187, 223895, 354912, 358332, 1948, 15092, 1640, 620705,
        //     312221, 480530, 677179, 302156, 64325, 412452, 641228, 53849, 53854, 393345, 167032, 320288, 293660, 533535,
        //     383498, 351460, 20352, 93456, 324852, 519182, 68718, 284052, 453395, 39920, 55508, 39921, 588648, 64690,
        //     329996, 493529, 35435, 44734, 9761, 568124, 14821, 524434, 15137, 22843, 75629, 283566, 857862, 713704,
        //     545609, 697843, 1141700, 385128, 756, 49948, 259316, 338952, 338953, 13804, 82992, 384018, 51497, 385687,
        //     755679, 54266, 522402, 40349, 56710, 626332, 513347, 40470, 6145, 40472, 16084, 83389, 109445, 1024604,
        //     1205983, 330457, 168259, 3131, 400928, 205584, 769, 12477, 118340, 283995, 447365, 457335, 899082, 672,
        //     12444, 12445, 674, 767, 675, 671, 673, 67823, 67824, 99089, 40533, 1620, 522931, 249070, 51540, 227159,
        //     10191, 82702, 638507, 166428, 4935, 1927, 425, 278154, 57800, 8355, 950, 260513, 1327821, 335977, 217, 89,
        //     87, 207932, 157336, 1726, 10138, 68721, 346364, 474350, 68620, 68622, 68625, 68630, 68633, 68986, 245891,
        //     324552, 458156, 603692, 730629, 475557, 889737, 617502, 8844, 512200, 353486, 69096, 120811, 449503,
        //     1215162, 40362, 1027159, 1995, 1996, 941, 942, 943, 944, 898789, 583, 72000, 72669, 72675, 72679, 193756,
        //     73139, 585511, 614587, 80863, 78139, 39954, 80405, 31051, 31050, 51943, 109572, 31049, 80857, 572616,
        //     154189, 26546, 567767, 31599, 404532, 491667, 31053, 80865, 1162244, 80868, 172591, 245928, 641909, 33176,
        //     80860, 80866, 59719, 15371, 31594, 50482, 80861, 39952, 32873, 30143, 45378, 251432, 53894, 31595, 1162239,
        //     76190, 101, 438435, 102651, 420809, 76535, 211387, 253980, 119569, 76122, 592687, 576743, 583209, 592688,
        //     592689, 491633, 259910, 433, 400650, 464566, 82, 73627, 954, 575264, 353081, 56292, 177677, 575265, 955,
        //     956, 40024, 40144, 73690, 407436, 762509, 337401, 11452, 2059, 6637, 81, 74176, 913673, 21832, 18491, 18510,
        //     58995, 74530, 317091, 21057, 402900, 161, 298, 163, 40369, 311, 1018494, 872585, 411999, 40370, 676, 75121,
        //     11114, 285, 58, 166426, 1865, 22, 10530, 13761, 10991, 12600, 447404, 33875, 16808, 12599, 115223, 303903,
        //     227679, 350499, 436931, 150213, 571891, 662708, 494407, 382190, 88557, 39057, 34065, 47292, 36218, 34067,
        //     10228, 25961, 50087, 75658, 31102, 15283, 75975, 76006, 296917, 452015, 76009, 76015, 40372, 40163, 39842,
        //     40164, 13836, 85, 404368, 44896, 2062, 527774, 1576, 35791, 1577, 133121, 1083862, 13648, 7737, 71679,
        //     173897, 400136, 14822, 1892, 54270, 330459, 77566, 40033, 54281, 870518, 77609, 4232, 646385, 4233, 4234,
        //     41446, 1159559, 934433, 11249, 355131, 1036561, 566525, 484886, 45745, 79253, 79256, 46862, 39887, 454626,
        //     675353, 939243, 1219926, 974691, 993729, 1190012, 557, 558, 559, 969681, 569094, 911916, 429617, 315635,
        //     324857, 634649, 129, 40209, 11, 1893, 1894, 1895, 140607, 181808, 181812, 40212, 533533, 20526, 37933,
        //     38757, 438747, 505945, 413279, 441829, 133701, 40216, 421611, 280, 296, 87101, 534, 290859, 90912, 92010,
        //     1930, 102382, 153518, 454640, 1300926, 24428, 10957, 890771, 39888, 40046, 39853, 15370, 53002, 339988,
        //     11873, 18357, 591, 94419, 426814, 55533, 1891, 444193, 9799, 9615, 337339, 69116, 10948, 9948, 57233,
        //     198375, 336000, 105864, 774752, 340270, 390043, 49051, 122917, 57158, 93014, 456048, 302150, 990, 774825,
        //     1724, 9806, 40232, 454983, 583083, 727745, 339404, 8698, 8587, 420818, 11430, 9732, 10144, 10898, 13676,
        //     72668, 206171, 39892, 120, 122, 121, 647250, 708702, 609681, 603, 604, 624860, 605, 40234, 40450, 73632,
        //     73676, 552688, 40235, 282035, 39894, 227783, 335777, 12924, 758323, 11319, 11135, 433808, 51739, 843241,
        //     989937, 1064835, 507569, 372343, 413644, 9078, 149871, 16692, 16693, 456538, 1098160, 218, 40239, 916192,
        //     46852, 416160, 405775, 149870, 986070, 39896, 40457, 401898, 10195, 616037, 284053, 76338, 42779, 605886,
        //     587807, 338970, 862, 863, 10193, 301528, 1084244, 1858, 91314, 38356, 8373, 335988, 40250, 94177, 97, 97917,
        //     3536, 912502, 14160, 10032, 19824, 397415, 335983, 580489, 912649, 533514, 700935, 10681, 13183, 894205,
        //     246741, 471014, 242828, 37797, 82690, 36657, 246655, 127585, 49538, 36668, 447399, 36658, 372058, 54279,
        //     54283, 42601, 24444, 54275, 40897, 54274, 54280, 54284, 54306, 18624, 18511, 54278, 54277, 54307, 54265,
        //     54276, 54273, 21410, 40644, 269149, 1084242
        // ];

        // int[] movieIds = await mediaContext.Movies
        //     .Select(selector: f => f.Id)
        //     .ToArrayAsync();

        // foreach (int id in movieIds) jobDispatcher.DispatchJob<AddMovieJob>(id, movieLibrary);

        // int[] tvIds = await mediaContext.Tvs
        //     // .Where(predicate: f => f.Id > 1404)
        //     .Select(selector: f => f.Id)
        //     .ToArrayAsync();

        // foreach (int id in tvIds) jobDispatcher.DispatchJob<AddShowJob>(id, tvLibrary);

        // Logger.System("Starting FileSystem Watcher", LogEventLevel.Debug);
        //
        // MediaContext mediaContext = new();
        // List<Library> libraries = await mediaContext.Libraries
        //     .Include(library => library.FolderLibraries)
        //     .ThenInclude(folderLibrary => folderLibrary.Folder)
        //     .ToListAsync();

        // foreach (Library library in libraries)
        // {
        //     List<string> paths = library.FolderLibraries.Select(folderLibrary => folderLibrary.Folder.Path).ToList();
        //     await Task.Run(() =>
        //     {
        //         var watch = fs.Watch(paths);
        //         Logger.System($"Watching {paths.Count} paths", LogEventLevel.Debug);
        //         
        //         new Task(() =>
        //         {
        //             Task.Delay(20000).Wait();
        //             
        //             foreach (var dispose in watch)
        //             {
        //                 dispose();
        //             }
        //         }).Start();
        //     });
        // }

        // Parallel.ForEach(tasks, task => task.Start());

        await using MediaContext mediaContext = new();

        // var stream0 = new X264(VideoCodecs.H264Nvenc.Value)
        //     .SetScale(FrameSizes._4k.Width, -2)
        //     .SetConstantRateFactor(20)
        //     .AllowHdr()
        //     .SetHlsSegmentFilename(":type:_:framesize:/:type:_:framesize:")
        //     .SetHlsPlaylistFilename(":type:_:framesize:/:type:_:framesize:")
        //     .SetColorSpace(ColorSpaces.Yuv444p)
        //     .SetPreset(VideoPresets.Slow)
        //     .SetTune(VideoTunes.Hq)
        //     .AddOpts("no-scenecut")
        //     .AddOpts("keyint=48")
        //     .AddCustomArgument("-x264opts", "no-scenecut")
        //     .AddCustomArgument("-crf", 52);
        
        // var stream1 = new X264(VideoCodecs.H264.Value)
        //     .SetScale(FrameSizes._1080p.Width)
        //     .SetConstantRateFactor(20)
        //     .AllowHdr()
        //     .SetHlsSegmentFilename(":type:_:framesize:/:type:_:framesize:")
        //     .SetHlsPlaylistFilename(":type:_:framesize:/:type:_:framesize:")
        //     .SetColorSpace(ColorSpaces.Yuv444p)
        //     .SetPreset(VideoPresets.Slow)
        //     .SetTune(VideoTunes.Film)
        //     .AddOpts("no-scenecut")
        //     .AddOpts("keyint", 48)
        //     .AddCustomArgument("-x264opts", "no-scenecut");
        
        var stream2 = new X264(VideoCodecs.H264Nvenc.Value)
            .SetScale(FrameSizes._4k.Width, -2)
            .SetConstantRateFactor(20)
            .ConvertHdrToSdr()
            .SetHlsSegmentFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetHlsPlaylistFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetColorSpace(ColorSpaces.Yuv420p)
            .SetPreset(VideoPresets.Fast)
            .SetTune(VideoTunes.Hq)
            .AddOpts("no-scenecut")
            .AddOpts("keyint=48")
            .AddCustomArgument("-x264opts", "no-scenecut");
        
        var stream3 = new X264(VideoCodecs.H264Nvenc.Value)
            .SetScale(FrameSizes._1080p.Width)
            .SetConstantRateFactor(20)
            .ConvertHdrToSdr()
            .SetHlsSegmentFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetHlsPlaylistFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetColorSpace(ColorSpaces.Yuv420p)
            .SetPreset(VideoPresets.Fast)
            .SetTune(VideoTunes.Hq)
            .AddOpts("no-scenecut")
            .AddOpts("keyint", 48)
            .AddCustomArgument("-x264opts", "no-scenecut");
        
        var stream4 = new Aac()
            .SetAudioChannels(2)
            .SetAllowedLanguages([Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita])
            .SetHlsSegmentFilename(":type:_:language:_:codec:/:type:_:language:_:codec:")
            .SetHlsPlaylistFilename(":type:_:language:_:codec:/:type:_:language:_:codec:");
        //
        // var stream5 = new DolbyDigitalPlus()
        //     .SetAllowedLanguages([Languages.Eng])
        //     .SetHlsSegmentFilename(":type:_:language:_:codec:/:type:_:language:_:codec:")
        //     .SetHlsPlaylistFilename(":type:_:language:_:codec:/:type:_:language:_:codec:");
        
        // var stream6 = new DolbyDigital()
        //     .SetAllowedLanguages([Languages.Eng])
        //     .SetHlsSegmentFilename(":type:_:language:_:codec:/:type:_:language:_:codec:")
        //     .SetHlsPlaylistFilename(":type:_:language:_:codec:/:type:_:language:_:codec:");
        
        var stream7 = new Sprite()
            .SetScale(320)
            .SetFilename("thumbs_:framesize:");

        var stream8 = new Vtt()
            .SetAllowedLanguages([Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita, Languages.Spa, Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara, Languages.Hin, Languages.Tel, Languages.Tam, Languages.Mal, Languages.Kan, Languages.Guj, Languages.Mar, Languages.Ben, Languages.Pan, Languages.Ori, Languages.Urd, Languages.Tur, Languages.Vie, Languages.Lao, Languages.Khm, Languages.Fil, Languages.Ind, Languages.Swa, Languages.Som, Languages.Tir])
            .SetHlsSegmentFilename(":type:_:language:_:codec:/_:language:_:codec:")
            // .SetHlsPlaylistFilename(":type:_:language:_:codec:/:type:_:language:_:codec:");
            .SetHlsPlaylistFilename("subtitles/Black.Panther.Wakanda.Forever.(2022).NoMercy.:language:.:variant:");
        
        var container = new Hls()
            .SetHlsFlags("independent_segments")
        // .AddStream(stream0)
        // .AddStream(stream1)
        // .AddStream(stream2)
        // .AddStream(stream3)
        // .AddStream(stream4)
        // .AddStream(stream5)
        // .AddStream(stream6)
        // .AddStream(stream7)
        .AddStream(stream8);

        // var ffmpeg = new FfMpeg()
        // // .Open("G:\\Marvels\\Films\\Download\\Iron.Man.2.2010.2160p.US.BluRay.REMUX.HEVC.DTS-HD.MA.TrueHD.7.1.Atmos-FGT\\Iron.Man.2.2010.2160p.US.BluRay.REMUX.HEVC.DTS-HD.MA.TrueHD.7.1.Atmos-FGT.mkv");
        // .Open($"{AppFiles.TranscodePath}\\Black.Panther.Wakanda.Forever.2022.1080p.BluRay.REMUX.AVC.DTS-HD.MA.TrueHD.7.1.Atmos-FGT.mkv");
        // .Open("M:\\Films\\Films\\Sintel.(2010)\\original\\[SDR-HEVC] Sintel.mkv");
        // .Open($"{AppFiles.TranscodePath}\\[HDR-HEVC] Cosmos.Laundromat.S01E01.First.Cycle.mkv");

        // var movie = await mediaContext.Movies
        // // .FirstOrDefaultAsync(x => x.Id == 10138); // Iron Man 2
        // // .FirstOrDefaultAsync(x => x.Id == 45745); // Sintel
        // .FirstOrDefaultAsync(x => x.Id == 505642); // black panther wakanda forever
        // // .FirstOrDefaultAsync(x => x.Id == 358332); // Cosmos Laundromat
        // if (movie == null) return;
        //
        // var folder = await mediaContext.Folders
        //     .FirstOrDefaultAsync(x => x.Id == Ulid.Parse("01HQ5W67GRBPHJKNAZMDYKMVXA"));
        // if (folder == null) return;
        //
        // var folderName = movie.CreateFolderName();
        // var title = movie.CreateTitle();
        // var fileName = movie.CreateFileName();
        // // var basePath = Path.Combine(folder.Path, folderName);
        // // var basePath = $"{AppFiles.TranscodePath}\\ironman";
        // var basePath = $"{AppFiles.TranscodePath}\\Black.Panther.Wakanda.Forever.(2022)";
        // // var basePath = $"{AppFiles.TranscodePath}\\sintel";
        //
        // ffmpeg.SetBasePath(basePath);
        // ffmpeg.SetTitle(title);
        // ffmpeg.ToFile(fileName);
        //
        // ffmpeg.AddContainer(container);
        //
        // ffmpeg.Build();
        //
        // var fullCommand = ffmpeg.GetFullCommand();
        // Logger.Encoder(fullCommand);
        //
        // // var ffmpegFile = Path.Combine(AppFiles.CachePath, "ffmpeg.json");
        // // await File.WriteAllTextAsync(ffmpegFile, fullCommand);
        //
        // var progressMeta = new ProgressMeta()
        // {
        //     Id = movie.Id,
        //     Title = title,
        //     BaseFolder = basePath,
        //     // ShareBasePath = folder.Id + "/" + folderName,
        //     ShareBasePath = "/transcode/" + folderName,
        //     AudioStreams = container.AudioStreams.Select(x => $"{x.StreamIndex}:{x.Language}_{x.AudioCodec.SimpleValue}").ToList(),
        //     VideoStreams = container.VideoStreams.Select(x => $"{x.StreamIndex}:{x.Scale.W}x{x.Scale.H}_{x.VideoCodec.SimpleValue}").ToList(),
        //     SubtitleStreams = container.SubtitleStreams.Select(x => $"{x.StreamIndex}:{x.Language}_{x.SubtitleCodec.SimpleValue}").ToList(),
        //     HasGpu = container.VideoStreams.Any(x =>
        //         x.VideoCodec.Value == VideoCodecs.H264Nvenc.Value || x.VideoCodec.Value == VideoCodecs.H265Nvenc.Value),
        //     IsHDR = container.VideoStreams.Any(x => x.IsHdr),
        // };
        //
        // var result = await ffmpeg.Run(fullCommand, basePath, progressMeta);
        // Logger.Encoder(result);
        //
        // // await stream7.BuildSprite(progressMeta);
        // //
        // container.BuildMasterPlaylist();

        // await Task.Run(async () =>
        // {
        //     await Task.Delay(10000);
        //
        //     Process process = new();
        //     process.StartInfo = new ProcessStartInfo
        //     {
        //         WindowStyle = ProcessWindowStyle.Hidden,
        //         FileName = AppFiles.FfmpegPath,
        //         Arguments = "/C -h encoder=webp",
        //         RedirectStandardOutput = true,
        //         UseShellExecute = false,
        //     };
        //
        //     process.Start();
        // });


        // await using MediaContext mediaContext = new();
        // var images = await mediaContext.Images
        //     .Where(image => image.Type == "backdrop" && image.FilePath != null && image.Width > 1920 && image.Height > 1080)
        //     .Where(image => image.VoteAverage > 5)
        //     .Where(image => image.MovieId != null || image.TvId != null)
        //     .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
        //                 e.Iso6391 != CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        //     // .GroupBy(image => image.MovieId ?? image.TvId)
        //     .ToListAsync();
        //
        // Logger.LoggerConfig.Information(images.Take(5).ToJson());

        // await Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var images = await mediaContext.Images
        //         .Where(image => image.Type == "backdrop" && image.FilePath != null && image.Width > 1920 && image.Height > 1080)
        //         .Where(image => image.VoteAverage > 5)
        //         .Where(image => image.MovieId != null || image.TvId != null)
        //         .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
        //                     e.Iso6391 != CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        //         // .GroupBy(image => image.MovieId ?? image.TvId)
        //         .ToListAsync();
        //
        //     Logger.App($"Processing {images.Count} Episode images");
        //     
        //     var day = new List<ScreenSaver>();
        //
        //     foreach (var image in images)
        //     {
        //         day.Add(new ScreenSaver{
        //             Path = Path.Combine(AppFiles.ImagesPath, "original", image.FilePath!.Replace("/", "")),
        //         });
        //     }
        //     
        //     await Ticker(day.Shuffle().ToList());
        // });

        // await Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var images = await mediaContext.Images
        //         .Where(image => image.Type == "backdrop" && image.FilePath != null)
        //         .Where(image => image.MovieId != null || image.TvId != null)
        //         .Where(e => e.Iso6391 != null && e.Iso6391 != "en" && e.Iso6391 != "" &&
        //                     e.Iso6391 != CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        //         .ToListAsync();
        //
        //     Logger.App($"Processing {images.Count} Episode images");
        //
        //     foreach (var image in images)
        //     {
        //         if(image.FilePath == null) continue;
        //         
        //         var path = Path.Combine(AppFiles.ImagesPath, "original", image.FilePath.Replace("/", ""));
        //         // Logger.App(path);
        //         if (!Path.Exists(path))
        //         {
        //             Logger.App($"Image not found: {path}");
        //             mediaContext.Images.Remove(image);
        //             // File.Delete(path);
        //         }
        //     }
        //     
        //     await mediaContext.SaveChangesAsync();
        // });

        // var input = await process.StandardOutput.ReadToEndAsync();
        //     
        // await process.WaitForExitAsync();
        // input = input.Replace(Environment.NewLine, "\r\n");
        //     
        //     await File.WriteAllTextAsync("./output.txt", input);
        //
        // var pattern = @"Encoder (?<Encoder>\w+)\s\[(?<EncoderName>.*)\](?:.*[\r\n]{2}\s+)+?Supported pixel formats:\s(?<PixelFormats>.*)[\r\n]{2}.+\sAVOptions:[\r\n]{2}(?<Options>\s{2}(?<Options_flag>\-[\w-]+)\s+(?<Options_Type><\w*>|\d)\s+(?<Options_Codecs>[DEVASIL\.]{11})\s(?<Options_Description>.*)?[\r\n]{2}){2,}";
        // string pattern = @"Encoder (?<Encpder>\w+)\s\[(?<EncoderName>.*)\](?:.*[\r\n]\s+)+?Supported pixel formats:\s(?<PixelFormats>.*)[\r\n].+\sAVOptions:[\r\n](?<Options>\s{2}(?<Options_flag>\-[\w-]+)\s+(?<Options_Type><\w*>|\d)\s+(?<Options_Codecs>[DEVASIL\.]{11})\s(?<Options_Description>.*)[\r\n]((?<Options_Type_Values>\s{5}(?<Options_Type_Values_Name>[\w\d\.-]+)\s+(?<Options_Type_Values_Value>-\d+|\d+|\w+)\s+(?<Options_Type_Values_Codecs>[DEVASIL\.]{11})(?<Options_Type_Values_Description>.*)[\r\n])+)?)+";

        // var regex = new Regex(pattern, RegexOptions.Multiline,TimeSpan.FromMinutes(100));
        // var matches = regex.Matches(input);
        // Logger.Encoder(matches);

        //     var regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline, TimeSpan.FromSeconds(300));
        //     var matches = regex.Matches(input);
        //     Logger.Encoder(input);
        // 
        // foreach (Match m in matches)
        // {
        //     Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
        // }
        // });

        // var artistClient = new ArtistClient();
        // var result = artistClient.ByMusicBrainzId(new Guid("056e4f3e-d505-4dad-8ec1-d04f521cbb56")).Result;
        // Logger.App(result?.Descriptions);
        //
        // var releaseGroupClient = new ReleaseGroupClient();
        // var result2 = releaseGroupClient.ByMusicBrainzId(new Guid("f9e8042a-674e-3f01-80ec-7f0ab1c537df")).Result;
        // Logger.App(result2?.Descriptions);

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var collections = mediaContext.Collections
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {collections.Count} Collection images");
        //
        //     foreach (var collection in collections)
        //     {
        //         if (collection is not { _colorPalette: "" }) continue;
        //
        //         Logger.Queue($"Fetching color palette for Collection Images {collection.Title}");
        //
        //         var palette =
        //             await ImageLogic2.GenerateColorPalette(collection.Poster, collection.Backdrop, download: true);
        //         collection._colorPalette = palette;
        //
        //         await mediaContext.SaveChangesAsync();
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var people = mediaContext.People
        //         .Where(x => x.Profile != null)
        //         .Where(x => x.Profile != "" && x.Profile != null)
        //         .ToList();
        //
        //     Logger.App($"Processing {people.Count} People images");
        //
        //     foreach (var person in people)
        //     {
        //         if (person is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(person.Id, "person");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var movies = mediaContext.Movies
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {movies.Count} Movie images");
        //
        //     foreach (var movie in movies)
        //     {
        //         if (movie is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(movie.Id, "movie");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var recommendations = mediaContext.Recommendations
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {recommendations.Count} Recommendation images");
        //
        //     foreach (var recommendation in recommendations)
        //     {
        //         if (recommendation is not { _colorPalette: "" }) continue;
        //         
        //         var type = recommendation.MovieFromId.HasValue ? "movie" : "tv";
        //         var id = recommendation.MovieFromId ?? recommendation.TvFromId ?? 0;
        //
        //         var colorPaletteJob = new ColorPaletteJob(id: id, model: "recommendation", type: type);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var similars = mediaContext.Similar
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {similars.Count} Similar images");
        //
        //     foreach (var similar in similars)
        //     {
        //         if (similar is not { _colorPalette: "" }) continue;
        //         
        //         var type = similar.MovieFromId.HasValue ? "movie" : "tv";
        //         var id = similar.MovieFromId ?? similar.TvFromId ?? 0;
        //
        //         var colorPaletteJob = new ColorPaletteJob(id: id, model: "similar", type: type);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var tvs = mediaContext.Tvs
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {tvs.Count} TvShow images");
        //
        //     foreach (var tv in tvs)
        //     {
        //         if (tv is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(tv.Id, "tv");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 4);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var seasons = mediaContext.Seasons
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {seasons.Count} Season images");
        //
        //     foreach (var season in seasons)
        //     {
        //         if (season is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(season.Id, "season");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 3);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var episodes = mediaContext.Episodes
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {episodes.Count} Episode images");
        //
        //     foreach (var episode in episodes)
        //     {
        //         if (episode is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(episode.Id, "episode");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var images = await mediaContext.Images
        //         .Where(x => x.Iso6391 == "en" || x.Iso6391 == null ||
        //                     x.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        //         .Where(x => x._colorPalette == "" && !x.FilePath!.Contains(".svg"))
        //         .Where(x => !x.ArtistId.HasValue)
        //         .Where(x => !x.AlbumId.HasValue)
        //         .ToListAsync();
        //
        //     Logger.App($"Processing {images.Count} images");
        //
        //     foreach (var image in images)
        //     {
        //         if (image is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob =
        //             new ColorPaletteJob(image.FilePath, "image", image.Iso6391);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 1);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var albums = await mediaContext.Albums
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //
        //     foreach (var album in albums)
        //     {
        //         if (album is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new MusicColorPaletteJob(album.Id.ToString(), "album");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var artists = await mediaContext.Artists
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //     
        //     foreach (var artist in artists)
        //     {
        //         if (artist is not {_colorPalette: "" }) continue;
        //         
        //         var colorPaletteJob = new MusicColorPaletteJob(id: artist.Id.ToString(), model: "artist");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var tracks = await mediaContext.Tracks
        //         .Include(x => x.AlbumTrack)
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //     
        //     foreach (var track in tracks)
        //     {
        //         if (track is not {_colorPalette: "" }) continue;
        //
        //         if (track.Cover is null) continue;
        //         
        //         var colorPaletteJob = new MusicColorPaletteJob(id: track.Id.ToString(), model: "track");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });
    }
    //
    // public static string GetDominantColor(string path)
    // {
    //     using var image = SixLabors.ImageSharp.Image.Load<Rgb24>(path);
    //     image.Mutate(
    //         x => x
    //             // Scale the image down preserving the aspect ratio. This will speed up quantization.
    //             // We use nearest neighbor as it will be the fastest approach.
    //             .Resize(new ResizeOptions()
    //             {
    //                 Sampler = KnownResamplers.NearestNeighbor, 
    //                 Size = new SixLabors.ImageSharp.Size(100, 0)
    //             })
    //             // Reduce the color palette to 1 color without dithering.
    //             .Quantize(new OctreeQuantizer
    //             {
    //                 Options =
    //                 {
    //                     MaxColors = 1, 
    //                     Dither = new OrderedDither(1),
    //                     DitherScale = 1
    //                 }
    //             }));
    //
    //     var dominant = image[0, 0];
    //         
    //     return dominant.ToHexString();
    //
    // }
    // private static int _day = 0;
    // // private const int TotalSecondsInDay = 24 * 60 * 60;
    //
    // private class ScreenSaver
    // {
    //     public string Path { get; set; } = "";
    //     // public string Color { get; set; } = "#000000";
    // }
    //
    // private static async Task Ticker(List<ScreenSaver> list)
    // {
    //     Logger.App("Starting Ticker");
    //
    //     // var intervalSeconds = TotalSecondsInDay / list.Count;
    //     using var itemTimer = new PeriodicTimer(TimeSpan.FromSeconds(300));
    //     
    //     _day = new Random().Next(0, list.Count);
    //
    //     do
    //     {
    //         if (_day >= list.Count)
    //         {
    //             _day = 0;
    //         }
    //
    //         var path = list[_day];
    //
    //         Logger.App("New Item: " + path.Path);
    //
    //         var color = GetDominantColor(Path.Combine(AppFiles.ImagesPath, "original", path.Path.Replace("/", "")));
    //
    //         Wallpaper.SilentSet(path.Path, WallpaperStyle.Fit, color);
    //
    //         _day++;
    //     }
    //     while (await itemTimer.WaitForNextTickAsync());
    // }
}
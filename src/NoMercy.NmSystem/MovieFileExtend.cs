using System.Text.RegularExpressions;

namespace NoMercy.NmSystem;
public class MovieFileExtend
{
    public string? Title { get; init; }
    public string? Year { get; init; }
    public bool IsSeries { get; set; }
    public int? Season { get; init; }
    public int? Episode { get; init; }
    public bool IsSuccess { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public int DiscNumber
    {
        get
        {
            if (Title is null) return 0;
            string pattern = @"^((?<discNumber>\d+)(-|\s))?(?<trackNumber>\d+)";
            Match match = Regex.Match(Title, pattern);
            return match.Groups["discNumber"].Success ? int.Parse(match.Groups["discNumber"].Value) : 0;
        }
    }
    
    public int TrackNumber
    {
        get
        {
            if (Title is null) return 0;
            string pattern = @"^((?<discNumber>\d+)-)?(?<trackNumber>\d+)";
            Match match = Regex.Match(Title, pattern);
            return match.Groups["trackNumber"].Success ? int.Parse(match.Groups["trackNumber"].Value) : 0;
        }
    }
}
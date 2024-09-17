using System.Globalization;
using System.Text.RegularExpressions;

namespace NoMercy.NmSystem;

public static class StringParser
{
    private static string _parseTitleSort(string? value = null, DateTime? date = null)
    {
        if (value == null) return "";

        value = Regex.Replace(value, "^The[\\s]*", "");
        value = Regex.Replace(value, "^An[\\s]{1,}", "");
        value = Regex.Replace(value, "^A[\\s]{1,}", "");
        value = Regex.Replace(value, @":\s|\sand\sthe", date != null ? $".{date.ParseYear()}." : ".");
        value = Regex.Replace(value, "\\.", " ");
        value = CleanFileName(value);

        return value.ToLower();
    }

    // public static string TitleSort<T>(this T? self, DateTime? date = null)
    // {
    //     return _parseTitleSort(self?.ToString(), date);
    // }

    private static string _cleanFileName(string? name)
    {
        if (name == null) return "";

        name = Regex.Replace(name, "/", ".");
        name = Regex.Replace(name, ":\\s", ".");
        name = Regex.Replace(name, "\\s", ".");
        name = Regex.Replace(name, "\\? {2}", ".");
        name = Regex.Replace(name, "\\? ", ".");
        name = Regex.Replace(name, "u,\\.", ".");
        name = Regex.Replace(name, "u, ", ".");
        name = Regex.Replace(name, "`", "");
        name = Regex.Replace(name, "'", "");
        name = Regex.Replace(name, "’", "");
        name = Regex.Replace(name, "\"", "");
        name = Regex.Replace(name, "u,", ".");
        name = Regex.Replace(name, "\"", "'");
        name = Regex.Replace(name, "\\. {2,}", ".");
        name = Regex.Replace(name, "\\s", ".");
        name = Regex.Replace(name, "&", "and");
        name = Regex.Replace(name, "#", "%23");
        name = Regex.Replace(name, "!", "");
        name = Regex.Replace(name, "\\*", "-");
        name = Regex.Replace(name, @"\.\.", ".");
        name = Regex.Replace(name, "u,\\.", ".");
        name = Regex.Replace(name, ": ", ".");
        name = Regex.Replace(name, ":", ".");
        name = Regex.Replace(name, "\\. *$", "");
        name = Regex.Replace(name, @"'|\?|\.\s|-\.|\.\(\d {1,3}\)|[^[:print:]\]|[^-_.[:alnum:]\]", "");
        name = Regex.Replace(name, "\\. {2,}", ".");

        return name.Trim();
    }

    public static string CleanFileName(this string? self)
    {
        return _cleanFileName(self);
    }

    public static string TitleSort(this object self, int? parseYear)
    {
        return _parseTitleSort(self.ToString(), parseYear != null ? new DateTime(parseYear.Value, 1, 1) : null);
    }

    // public static string? TitleCase<T>(this T? self)
    // {        
    //     return self?.ToString()?.Split("")[0].ToUpper() + self?.ToString()?.Substring(1).ToLower() ?? "";
    // }

    public static string Capitalize(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str.Substring(1);
    }

    public static string ToTitleCase(this string str, string culture = "en-US")
    {
        if (string.IsNullOrEmpty(str))
            return str;

        TextInfo textInfo = new CultureInfo(culture, false).TextInfo;
        return textInfo.ToTitleCase(str.ToLower());
    }

    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        string[] words = str.Split(new[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("_", words.Select(word => word[..1].ToUpper() + word[1..].ToLower()));
    }

    public static string ToUcFirst(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str[1..].ToLower();
    }

    public static int ToSeconds(this string hms)
    {
        if (string.IsNullOrEmpty(hms)) return 0;

        int[] parts = hms.Split(':').Select(int.Parse).ToArray();
        if (parts.Length < 3) parts = new[] { 0 }.Concat(parts).ToArray();

        return parts[0] * 60 * 60 + parts[1] * 60 + parts[2];
    }

    public static string TitleSort<T>(this T? self, DateTime? date = null)
    {
        return _parseTitleSort(self?.ToString(), date);
    }
}
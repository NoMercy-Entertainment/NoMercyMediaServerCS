using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NoMercy.Helpers;

public static partial class Str
{
    public static double MatchPercentage(string strA, string strB)
    {
        var result = 0;
        for (var i = strA.Length - 1; i >= 0; i--)
            if (i >= strB.Length || strA[i] == strB[i])
            {
                // Do nothing
            }
            else if (char.ToLower(strA[i]) == char.ToLower(strB[i]))
            {
                result += 1;
            }
            else
            {
                result += 4;
            }

        return 100 - (result + 4 * Math.Abs(strA.Length - strB.Length)) / (2.0 * (strA.Length + strB.Length)) * 100;
    }

    public static List<T> SortByMatchPercentage<T>(IEnumerable<T> array, Func<T, string> keySelector, string match)
    {
        return array.OrderBy(item => MatchPercentage(match, keySelector(item))).ToList();
    }

    public static string RemoveAccents(this string s)
    {
        var destEncoding = Encoding.GetEncoding("iso-8859-8");

        return destEncoding.GetString(
            Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
    }

    public static string RemoveDiacritics(this string text)
    {
        var formD = text.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new();

        foreach (var ch in formD)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string RemoveNonAlphaNumericCharacters(this string text)
    {
        return Regex.Replace(text, @"[^a-zA-Z0-9\s.-]", "");
    }

    [GeneratedRegex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\W(1(8|9)|20)\d{2})")]
    public static partial Regex MatchYearRegex();
    
    public static string PathName(this string path)
    {
        return Regex.Replace(path, @"[\/\\]", Path.DirectorySeparatorChar.ToString());
    }
    
    public static int ToInt(this string path)
    {
        return int.Parse(path);
    }
}
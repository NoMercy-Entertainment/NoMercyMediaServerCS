using System.Globalization;
using Cliver;

namespace NoMercy.Helpers;

public static class DateTimeParser
{
    private static int _parseYear(DateTime? dateString = null)
    {
        if (string.IsNullOrEmpty(dateString.ToString())) return new DateTime().Year;

        DateTime date;
        try
        {
            date = DateTime.ParseExact(dateString.ToString() ?? string.Empty, "d-M-yyyy HH:mm:ss",
                CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            date = DateTime.Parse(dateString.ToString() ?? string.Empty, CultureInfo.InvariantCulture);
        }

        return date.Year;
    }

    public static int ParseYear(this DateTime? self)
    {
        try
        {
            return string.IsNullOrEmpty(self.ToString())
                ? 0
                : _parseYear(self);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static int ParseYear(this DateTime self)
    {
        try
        {
            return string.IsNullOrEmpty(self.ToString(CultureInfo.InvariantCulture))
                ? 0
                : _parseYear(self);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static string ToHms(this int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString();
    }

    public static DateTime? ParseDateTime(string? value)
    {
        string[] validFormats =
        [
            "yyyy",
            "MM-yyyy",
            "dd-MM-yyyy",
            "dd-MM-yyyy HH:mm:ss",
            "yyyy-MM",
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss",
        ];

        var success = DateTime.TryParseExact(value, validFormats,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);

        if (success && result != DateTime.MinValue) return result;
        
        DateTimeRoutines.ParsedDateTime pdt;
        var success2 = value.TryParseDate(DateTimeRoutines.DateTimeFormat.USA_DATE, out pdt);
        if (success2 && pdt.DateTime.Date != DateTime.MinValue) return pdt.DateTime.Date;
        
        return null;
    }
}
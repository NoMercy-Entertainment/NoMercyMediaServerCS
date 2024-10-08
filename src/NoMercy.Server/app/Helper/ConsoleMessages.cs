using System.Text;
using NoMercy.Networking;
using NoMercy.NmSystem;
using Pastel;

namespace NoMercy.Server.app.Helper;

public abstract class ConsoleMessages
{
    private static string[] Colors => ApiInfo.Colors;

    private static string Quote => ApiInfo.Quote;

    public static Task ServerRunning()
    {
        ConsoleExtensions.Enable();

        Console.WriteLine(("╔" + Repeat("═", 46) + "╗").Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + @"     " +
                          "Secure Server running: on port:".Pastel("#5ffa71") +
                          $" {Config.InternalServerPort}     ".Pastel("#ffffff") +
                          $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + @"      " + "  visit:".Pastel("#cccccc") +
                          $"  {Config.AppBaseUrl}       ".Pastel("#ffffff") + $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine(("╚" + Repeat("═", 46) + "╝").Pastel("#00a10d"));

        return Task.CompletedTask;
    }

    private static string _(string? color = null)
    {
        return "║".Pastel(color ?? Colors[0]);
    }

    private static string Repeat(string stringToRepeat, int repeat)
    {
        StringBuilder builder = new(repeat * stringToRepeat.Length);
        for (int i = 0; i < repeat; i++) builder.Append(stringToRepeat);

        return builder.ToString();
    }

    private static string CreateQuote(string text, int totalSize, int rightPadding)
    {
        // if (text.Length + rightPadding > totalSize - 2) throw new Exception("The text is too long to fit in the quote");

        List<string> spacing = [];

        // Add spaces to the left of the text
        for (int i = 102 - rightPadding; i > text.Length; i--) spacing.Add("");

        spacing.Add(text);

        // Add spaces to the right of the text
        for (int i = 0; i < rightPadding; i++) spacing.Add("");

        return string.Join(" ", spacing);
    }

    public static Task Logo()
    {
        StringBuilder builder = new();
        string outputString = "║  NoMercy MediaServer  ║";
        int totalWidth = 0;

        for (int i = 0; i < ConsoleLetters.Colossal.FirstOrDefault().Value.Count - 1; i++)
        {
            foreach (char letter in outputString)
            {
                string? text = ConsoleLetters.Colossal[letter.ToString()][i];

                text = letter switch
                {
                    '║' => text.Pastel(Colors[0]),
                    'N' or 'M' or 'S' => text.Pastel(Colors[1]),
                    _ => text.Pastel(Colors[2])
                };

                builder.Append(text);

                if (i == 5) totalWidth += ConsoleLetters.Colossal[letter.ToString()][i].Length;
            }

            if (i == 9) continue;
            builder.AppendLine();
        }

        int magicSpacer = totalWidth - 2;

        Console.WriteLine($"{("╔" + Repeat("═", magicSpacer) + "╗").Pastel(Colors[0])}");
        Console.WriteLine($"{_()}{Repeat(" ", magicSpacer)}{_()}");

        Console.WriteLine(builder.ToString());

        Console.WriteLine($"{_()}{Repeat(" ", 63)}{ConsoleLetters.Colossal["y"][10].Pastel(Colors[2])}" +
                          CreateQuote(Quote, 0, 4) + $"{ConsoleLetters.Colossal["║"][0].Pastel(Colors[0])}");
        // Console.WriteLine($"{_()}" + CreateQuote(Quote, totalWidth, 4) + $"{ConsoleLetters.Colossal["║"][0].Pastel(Colors[0])}");        
        Console.WriteLine($"{("╚" + Repeat("═", magicSpacer) + "╝").Pastel(Colors[0])}");

        return Task.CompletedTask;
    }

    public static Task Welcome()
    {
        Console.WriteLine(("╔" + Repeat("═", 46) + "╗").Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + @"     " +
                          "Welcome to NoMercy MediaServer".Pastel("#5ffa71") +
                          $"     ".Pastel("#ffffff") +
                          $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + @"      " + "Version:".Pastel("#cccccc") +
                          "  1.0.0      ".Pastel("#ffffff") + $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine(("╚" + Repeat("═", 46) + "╝").Pastel("#00a10d"));

        return Task.CompletedTask;
    }
}
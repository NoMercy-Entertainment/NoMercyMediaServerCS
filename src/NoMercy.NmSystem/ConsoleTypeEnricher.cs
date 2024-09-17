using Pastel;
using Serilog.Core;
using Serilog.Events;
using System.Drawing;
using System.Text;

namespace NoMercy.NmSystem;
internal class ConsoleTypeEnricher : ILogEventEnricher
{
    private static string SpacerEnd(string text, int padding)
    {
        StringBuilder spacing = new();
        spacing.Append(text);
        for (int i = 0; i < padding - text.Length; i++) spacing.Append(' ');

        return spacing.ToString();
    }

    private static string SpacerBegin(string text, int padding)
    {
        StringBuilder spacing = new();
        for (int i = 0; i < padding - text.Length; i++) spacing.Append(' ');
        spacing.Append(text);

        return spacing.ToString();
    }

    private static string Spacer(string text, int padding, bool begin = false)
    {
        return begin ? SpacerBegin(text, padding) : SpacerEnd(text, padding);
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.Properties.TryGetValue("ConsoleType", out LogEventPropertyValue? value);

        string type = value?.ToString().Replace("\"", "") ?? "app";

        Color color = Logger.GetColor(type);

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(
            "ConsoleType", Spacer(Logger.Capitalize[type], 12, true).Pastel(color)));
    }
}
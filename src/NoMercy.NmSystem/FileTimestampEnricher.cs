using Serilog.Core;
using Serilog.Events;
using System.Globalization;

namespace NoMercy.NmSystem;
internal class FileTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        string timestamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

        logEvent.RemovePropertyIfPresent("@t");
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "Time", timestamp));
    }
}
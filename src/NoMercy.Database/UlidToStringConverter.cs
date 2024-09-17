using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NoMercy.Database;
public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints defaultHints = new(26);

    public UlidToStringConverter() : this(null)
    {
    }

    private UlidToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(
            x => x.ToString(),
            x => Ulid.Parse(x),
            defaultHints.With(mappingHints))
    {
    }
}
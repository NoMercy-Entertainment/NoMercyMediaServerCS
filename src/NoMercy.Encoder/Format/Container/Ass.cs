using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class Ass : BaseContainer
{
    public Ass() : base()
    {
        SetContainer(SubtitleContainers.Ass);
        AddCustomArgument("-f", SubtitleFormats.Ass);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        SubtitleCodecs.Ass
    ];
}
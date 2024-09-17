using Newtonsoft.Json;
using NoMercy.Database;

namespace NoMercy.Api.Controllers.Socket;
public class ColorPalette
{
    [JsonProperty("cover")] public PaletteColors Cover { get; set; } = new();
}
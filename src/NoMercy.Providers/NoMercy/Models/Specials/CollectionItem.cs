namespace NoMercy.Providers.NoMercy.Models.Specials;
public class CollectionItem
{
    public int index { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public int year { get; set; }
    public int[] seasons { get; set; }
    public int[] episodes { get; set; }
}
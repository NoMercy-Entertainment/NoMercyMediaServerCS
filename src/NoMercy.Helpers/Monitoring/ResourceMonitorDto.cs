namespace NoMercy.Helpers.Monitoring;
public class ResourceMonitorDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public float Total { get; set; }
    public float Available { get; set; }
    public float Percentage => Available / Total * 100;
}
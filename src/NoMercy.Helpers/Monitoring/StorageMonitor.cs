namespace NoMercy.Helpers.Monitoring;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public static class StorageMonitor
{
    public static List<ResourceMonitorDto> Main()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        List<ResourceMonitorDto> resourceMonitorDtos = [];

        foreach (DriveInfo d in allDrives)
        {
            ResourceMonitorDto resourceMonitorDto = new()
            {
                Name = d.Name,
                Type = d.DriveType.ToString()
            };
            if (d.IsReady)
            {
                resourceMonitorDto.Total = (float)d.TotalSize / 1024 / 1024 / 1024;
                resourceMonitorDto.Available = (float)d.AvailableFreeSpace / 1024 / 1024 / 1024;
            }

            resourceMonitorDtos.Add(resourceMonitorDto);
        }

        return resourceMonitorDtos;
    }
}
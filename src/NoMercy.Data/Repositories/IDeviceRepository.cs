using Microsoft.EntityFrameworkCore.Query;
using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface IDeviceRepository
{
    IIncludableQueryable<Device, ICollection<ActivityLog>> GetDevicesAsync();
    Task AddDeviceAsync(Device device);
    Task DeleteDeviceAsync(Device device);
}
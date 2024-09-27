// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//
// using System.Security.Claims;
// using System.Web.Http;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Newtonsoft.Json;
// using NoMercy.Database;
// using NoMercy.Database.Models;
// using NoMercy.Server.app.Helper;
// using NoMercy.Server.app.Http.Middleware;
// using Hub = Microsoft.AspNetCore.SignalR.Hub;
//
// namespace NoMercy.Server.app.Http.Controllers.Socket;
//
// [Authorize]
// public class ConnectionHub : Hub
// {
//     private readonly IHttpContextAccessor _httpContextAccessor;
//
//     protected ConnectionHub(IHttpContextAccessor httpContextAccessor)
//     {
//         _httpContextAccessor = httpContextAccessor;
//     }
//
//     public override async Task OnConnectedAsync()
//     {
//         var user = User();
//         if (user is null) return;
//
//         var accessToken = _httpContextAccessor.HttpContext?.Request.Query.FirstOrDefault(x => x.Key == "access_token")
//             .Value;
//         string[] result = accessToken.GetValueOrDefault().ToString().Split("&");
//
//         var client = new Client
//         {
//             Sub = user.Id,
//             Ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
//             Socket = Clients.Caller
//         };
//
//         foreach (var item in result)
//         {
//             string[] keyValue = item.Split("=");
//
//             if (keyValue.Length < 2) continue;
//
//             keyValue[1] = keyValue[1].Replace("+", " ");
//
//             switch (keyValue[0])
//             {
//                 case "access_token":
//                     continue;
//                 case "client_id":
//                     client.DeviceId = keyValue[1];
//                     break;
//                 case "client_name":
//                     client.Name = keyValue[1];
//                     break;
//                 case "client_type":
//                     client.Type = keyValue[1];
//                     break;
//                 case "client_version":
//                     client.Version = keyValue[1];
//                     break;
//                 case "client_os":
//                     client.Os = keyValue[1];
//                     break;
//                 case "client_browser":
//                     client.Browser = keyValue[1];
//                     break;
//                 case "client_device":
//                     client.Model = keyValue[1];
//                     break;
//                 case "custom_name":
//                     client.CustomName = keyValue[1];
//                     break;
//             }
//         }
//
//         await using MediaContext mediaContext = new();
//         await mediaContext.Devices.Upsert(client)
//             .On(x => x.DeviceId)
//             .WhenMatched((ds, di) => new Device
//             {
//                 Browser = di.Browser,
//                 // CustomName = di.CustomName,
//                 DeviceId = di.DeviceId,
//                 Ip = di.Ip,
//                 Model = di.Model,
//                 Name = di.Name,
//                 Os = di.Os,
//                 Type = di.Type,
//                 Version = di.Versionl,
//                 UpdatedAt = di.UpdatedAt
//             })
//             .RunAsync();
//
//         var device = mediaContext.Devices.FirstOrDefault(x => x.DeviceId == client.DeviceId);
//         
//         client.CustomName = device?.CustomName;
//
//         if (device is not null)
//         {
//             await mediaContext.ActivityLogs.AddAsync(new ActivityLog
//             {
//                 DeviceId = device.Id,
//                 Time = DateTime.Now,
//                 Type = "Connected to server",
//                 UserId = user.Id
//             });
//
//             await mediaContext.SaveChangesAsync();
//         }
//
//         Networking.SocketClients.TryAdd(Context.ConnectionId, client);
//
//         await Clients.All.SendAsync("ConnectedDevices", Devices());
//
//         await base.OnConnectedAsync();
//     }
//
//     protected User? User()
//     {
//         var userId = Context.User.UserId();
//         return TokenParamAuthMiddleware.Users.FirstOrDefault(x => x.Id == userId);
//     }
//
//     public override async Task OnDisconnectedAsync(Exception? exception)
//     {
//         if (Networking.SocketClients.TryGetValue(Context.ConnectionId, out var client))
//         {
//             await using MediaContext mediaContext = new();
//             var device = mediaContext.Devices.FirstOrDefault(x => x.DeviceId == client.DeviceId);
//             if (device is not null)
//             {
//                 await mediaContext.ActivityLogs.AddAsync(new ActivityLog
//                 {
//                     DeviceId = device.Id,
//                     Time = DateTime.Now,
//                     Type = "Disconnected from server",
//                     UserId = client.Sub
//                 });
//
//                 await mediaContext.SaveChangesAsync();
//             }
//
//             Networking.SocketClients.Remove(Context.ConnectionId, out _);
//
//             await Clients.All.SendAsync("ConnectedDevices", Devices());
//         }
//
//         await base.OnDisconnectedAsync(exception);
//     }
//
//     private List<Device> Devices()
//     {
//         var user = User();
//
//         return Networking.SocketClients.Values
//             .Where(x => x.Sub.Equals(user?.Id))
//             .Select(c => new Device
//             {
//                 Name = c.Name,
//                 Ip = c.Ip,
//                 DeviceId = c.DeviceId,
//                 Browser = c.Browser,
//                 Os = c.Os,
//                 Model = c.Model,
//                 Type = c.Type,
//                 Version = c.Version,
//                 Id = c.Id,
//                 CustomName = c.CustomName
//             })
//             .ToList();
//     }
// }
//
// public class ClientRequest
// {
//     [JsonProperty("id")] public string Id { get; set; }
//     [JsonProperty("browser")] public string Browser { get; set; }
//     [JsonProperty("os")] public string Os { get; set; }
//     [JsonProperty("device")] public string Device { get; set; }
//     [JsonProperty("custom_name")] public string CustomName { get; set; }
//     [JsonProperty("type")] public string Type { get; set; }
//     [JsonProperty("name")] public string Name { get; set; }
//     [JsonProperty("version")] public string Version { get; set; }
// }
//
// public class Client : Device
// {
//     public Guid Sub { get; set; }
//     public int? Ping { get; set; }
//     public ISingleClientProxy Socket { get; set; }
// }


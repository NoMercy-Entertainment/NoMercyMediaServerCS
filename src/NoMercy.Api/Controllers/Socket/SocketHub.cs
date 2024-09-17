using Microsoft.AspNetCore.Http;
using NoMercy.Networking;

namespace NoMercy.Api.Controllers.Socket;

public class SocketHub(IHttpContextAccessor httpContextAccessor) : ConnectionHub(httpContextAccessor);
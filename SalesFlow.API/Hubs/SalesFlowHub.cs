using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SalesFlow.API.Hubs
{
    [Authorize]
    public class SalesFlowHub : Hub
    {
    }
}

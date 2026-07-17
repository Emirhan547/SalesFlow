using Microsoft.AspNetCore.SignalR;
using SalesFlow.API.Hubs;
using SalesFlow.Business.Services.RealtimeServices;

namespace SalesFlow.API.Services;

public class SignalRRealtimeService
    : IRealtimeService
{
    private readonly IHubContext<SalesFlowHub> _hub;

    public SignalRRealtimeService(
        IHubContext<SalesFlowHub> hub)
    {
        _hub = hub;
    }

    public async Task ActivityLogCreatedAsync()
    {
        await _hub.Clients.All.SendAsync(
            "ActivityLogCreated");
    }

    public async Task DashboardUpdatedAsync()
    {
        await _hub.Clients.All.SendAsync(
            "DashboardUpdated");
    }

    public async Task SendNotificationAsync(
        int userId,
        object notification)
    {
        await _hub.Clients
            .User(userId.ToString())
            .SendAsync(
                "NotificationReceived",
                notification);
    }
}
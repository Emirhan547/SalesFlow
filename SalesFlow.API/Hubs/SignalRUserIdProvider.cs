using Microsoft.AspNetCore.SignalR;

namespace SalesFlow.API.Hubs
{
    public class SignalRUserIdProvider
        : IUserIdProvider
    {
        public string? GetUserId(
            HubConnectionContext connection)
        {
            return connection.User?
                .FindFirst("sub")?
                .Value;
        }
    }
}
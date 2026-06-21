using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RentThings.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public static string UserGroup(Guid userId) => $"user-{userId}";

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userId, out var id))
            await Groups.AddToGroupAsync(Context.ConnectionId, UserGroup(id));

        await base.OnConnectedAsync();
    }
}

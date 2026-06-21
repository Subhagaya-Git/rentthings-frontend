using Microsoft.AspNetCore.SignalR;
using RentThings.Api.DTOs;
using RentThings.Api.Hubs;

namespace RentThings.Api.Services.Azure;

public interface INotificationPublisher
{
    Task PublishAsync(Guid userId, NotificationDto notification, CancellationToken ct = default);
}

public class NotificationPublisher(IHubContext<NotificationHub> hub, ILogger<NotificationPublisher> logger) : INotificationPublisher
{
    public async Task PublishAsync(Guid userId, NotificationDto notification, CancellationToken ct = default)
    {
        var group = NotificationHub.UserGroup(userId);
        await hub.Clients.Group(group).SendAsync("ReceiveNotification", notification, ct);
        logger.LogInformation("SignalR notification sent to user {UserId}: {Title}", userId, notification.Title);
    }
}

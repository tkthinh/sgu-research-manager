using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> logger;
        public NotificationHub(ILogger<NotificationHub> logger)
        {
            this.logger = logger;
        }
    }
}
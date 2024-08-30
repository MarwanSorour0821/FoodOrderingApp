using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    // Method to notify all connected clients about new orders
    public async Task NotifyOrderUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveOrderUpdate", message);
    }
}
using Microsoft.AspNetCore.SignalR;

namespace BusinessLayer.Signals
{
    public class StockAlertHub : Hub
    {
        public async Task SendAlertTask(string message)
        {
            await Clients.All.SendAsync("ReceiveAlert", message);
        }
    }

}


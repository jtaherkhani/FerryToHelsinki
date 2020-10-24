using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FerryToHelsinki.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        public async Task SendMessage(string userName, string messageContents)
        {
            await Clients.All.SendMessage(userName, messageContents);
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }

    public interface IMessageClient
    {
        Task SendMessage(string userName, string messageContents);
    }
}

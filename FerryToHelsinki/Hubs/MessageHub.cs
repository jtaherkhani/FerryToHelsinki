using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FerryToHelsinki.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        public async Task SendMessage(string userName, string messageContents, string imageUrl)
        {
            await Clients.All.SendMessage(userName, messageContents, imageUrl);
        }

        public async Task StartGame(string startGameMessage)
        {
            await Clients.All.StartGame(startGameMessage);
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
        Task SendMessage(string userName, string messageContents, string imageUrl);

        Task StartGame(string startGameMessage);
    }
}

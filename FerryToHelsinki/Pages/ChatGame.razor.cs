using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages
{
    public partial class ChatGame
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool _gameStarted = false;
        private List<Message> _messages = new();
        private Message _message = new();

        private HubConnection _hubConnection;
        private const string HostUserName = "TeriyakiSupreme";

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
               .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
               .Build();

            _hubConnection.On<string, string>("SendMessage", (user, messageContents) => OnMessageReceived(user, messageContents));
            await _hubConnection.StartAsync();
        }

        public async Task StartGameAsync()
        {
            _gameStarted = true;
            await _hubConnection.SendAsync("StartGame", "Game Has Started");

            StateHasChanged();
        }

        public async Task SendFerryMessageAsync(Message newMessage)
        {
            if (!string.IsNullOrWhiteSpace(newMessage.MessageContents))
            {
                await _hubConnection.SendAsync("SendMessage", newMessage.UserName, newMessage.MessageContents);
                _message = new();
            }
        }

        private void OnMessageReceived(string user, string messageContents)
        {
            var message = new Message
            {
                UserName = user,
                MessageContents = messageContents
            };

            _messages.Add(message);
            StateHasChanged();
        }
    }
}

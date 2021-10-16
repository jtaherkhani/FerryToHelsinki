using FerryToHelsinki.Services;
using FerryToHelsinki.Singleton;
using FerryToHelsinkiWebsite.Data.Constants;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages
{
    public partial class ChatGame
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private AcceptMessagesSingleton AcceptMessagesSingleton { get; set; }

        [Inject]
        private BlobStorageService BlobStorageService { get; set; }

        private bool _gameStarted = false;
        private List<Message> _messages = new();
        private string _messageContents;
        private string _messageUrl;

        private HubConnection _hubConnection;
        private const string HostUserName = GameConstants.HostUserName;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
               .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
               .Build();

            _hubConnection.On<string, string, string>("SendMessage", (user, messageContents, imageUrl) => OnMessageReceived(user, messageContents, imageUrl));
            await _hubConnection.StartAsync();
        }

        public async Task StartGameAsync()
        {
            _gameStarted = true;
            AcceptMessagesSingleton.AcceptMessages = true;
            await _hubConnection.SendAsync("StartGame", "Game Has Started");

            StateHasChanged();
        }

        public async Task SendFerryMessageAsync(string newMessageContents)
        {
            if (!string.IsNullOrWhiteSpace(newMessageContents))
            {
                await _hubConnection.SendAsync("SendMessage", HostUserName, newMessageContents, _messageUrl);
                AcceptMessagesSingleton.AcceptMessages = true;
                _messageContents = string.Empty;
                _messageUrl = string.Empty;
            }
        }

        private void OnMessageReceived(string user, string messageContents, string imageUrl)
        {
            var message = new Message
            {
                UserName = user,
                MessageContents = messageContents,
                ImageUrl = imageUrl
            };

            _messages.Add(message);
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        private async Task LoadFilesAsync(InputFileChangeEventArgs e)
        {
            var chatImgUpload = e.GetMultipleFiles(1)[0];
            var url = await BlobStorageService.UploadBrowserFileAsync(chatImgUpload);

            _messageUrl = url;
            StateHasChanged();
        }

        private string IsFileUploaded =>
            (!string.IsNullOrWhiteSpace(_messageUrl)).ToString();
    }
}

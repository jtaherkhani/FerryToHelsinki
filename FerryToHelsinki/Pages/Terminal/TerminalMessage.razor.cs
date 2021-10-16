using FerryToHelsinki.Constants;
using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using FerryToHelsinki.Services;
using FerryToHelsinkiWebsite.Data.Constants;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class TerminalMessage
    {
        [Parameter]
        public MessageService MessageService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private HubConnection _hubConnection;
        private bool _hostSentLastMessage;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
               .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
               .Build();

            _hubConnection.On<string, string, string>("SendMessage", (user, messageContents, imageUrl) => OnMessageReceivedAsync(user, messageContents, imageUrl));
            await _hubConnection.StartAsync();
        }

        private async Task OnMessageReceivedAsync(string user, string messageContents, string imageUrl)
        {
            var message = new Message
            {
                UserName = user,
                MessageContents = messageContents,
                ImageUrl = imageUrl
            };

            if (!string.IsNullOrWhiteSpace(message.ImageUrl))
            {
                await HandleImageInMessage(message);
            }

            if (GameConstants.HostUserName == user && CurrentTerminalState == TerminalStates.FerryToHelsinkiGameplay)
            {
                await HandleHostUserMessage(message);
                _hostSentLastMessage = true;
            }
            else
            {
                await HandlePlayerMessage(message);
                _hostSentLastMessage = false;
            }
        }

        private async Task HandleImageInMessage(Message message)
        {
            await JsRuntime.InvokeVoidAsync("terminalFunctions.renderImage", message.ImageUrl);
        }

        private async Task HandleHostUserMessage(Message message)
        {
            await JsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", message.MessageContents, MessagePrefix, _hostSentLastMessage);
        }

        private async Task HandlePlayerMessage(Message message)
        {
            var result = await JsRuntime.InvokeAsync<bool>("terminalFunctions.animateMessage", message.MessageContents, _hostSentLastMessage);

            if (result && CurrentTerminalState == TerminalStates.Opened)
            {
                await MessageService.HandleMessageAsync(message);

                if (MessageService.CurrentTerminalState != CurrentTerminalState)
                {
                    await UpdateTerminalState(MessageService.CurrentTerminalState);
                }
            }
        }

        private string MessagePrefix =>
            CurrentTerminalState == TerminalStates.Opened ? MessageService.MessagePrompt : MessageConstants.MessagePrompt;
    }
}

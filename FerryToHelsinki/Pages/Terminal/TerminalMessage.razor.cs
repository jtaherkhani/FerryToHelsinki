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

            _hubConnection.On<string, string>("SendMessage", (user, messageContents) => OnMessageReceivedAsync(user, messageContents));
            await _hubConnection.StartAsync();
        }

        private async Task OnMessageReceivedAsync(string user, string messageContents)
        {
            var message = new Message
            {
                UserName = user,
                MessageContents = messageContents
            };

            if (GameConstants.HostUserName == user && CurrentTerminalState == TerminalStates.FerryToHelsinkiGameplay)
            {
                await HandleHostUserMessage(message);
            }
            else
            {
                await HandlePlayerMessage(message);
            }
        }

        private async Task HandleHostUserMessage(Message message)
        {
            await JsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", message.MessageContents, MessagePrefix);
        }

        private async Task HandlePlayerMessage(Message message)
        {
            var result = await JsRuntime.InvokeAsync<bool>("terminalFunctions.animateMessage", message.MessageContents);

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

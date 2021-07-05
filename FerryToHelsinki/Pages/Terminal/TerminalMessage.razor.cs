using FerryToHelsinki.Enums;
using FerryToHelsinki.Services;
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
        public EventCallback<TerminalStates> OnTerminalStateChanged { get; set; }

        [Parameter]
        public MessageService MessageService { get; set; }

        [Parameter] 
        public TerminalStates CurrentTerminalState { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
               .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
               .Build();

            hubConnection.On<string, string>("SendMessage", (user, messageContents) => OnMessageReceived(user, messageContents));
            await hubConnection.StartAsync();
        }

        private async Task OnMessageReceived(string user, string messageContents)
        {
            var message = new Message
            {
                UserName = user,
                MessageContents = messageContents
            };

            if (await JsRuntime.InvokeAsync<bool>("terminalFunctions.animateMessage", messageContents))
            {
                await MessageService.HandleMessageAsync(message);

                if (MessageService.CurrentTerminalState != CurrentTerminalState)
                {
                    await OnTerminalStateChanged.InvokeAsync(MessageService.CurrentTerminalState);
                }
            }
        }
    }
}

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
        public EventCallback<Message> OnMessageFound { get; set; }

        [Parameter]
        public string MessagePromptPrefix { get; set; } = "";

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
            await OnMessageFound.InvokeAsync(new Message
            {
                UserName = user,
                MessageContents = messageContents
            });

            await JsRuntime.InvokeVoidAsync("terminalFunctions.animateMessage", messageContents);
        }
    }
}

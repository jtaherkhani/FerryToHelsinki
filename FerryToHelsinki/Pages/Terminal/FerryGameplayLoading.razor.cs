using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryGameplayLoading
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
               .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
               .Build();

            _hubConnection.On<string>("StartGame", (messageContents) => StartGameAsync(messageContents));
            await _hubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldRenderForTerminalState)
            {
                await JsRuntime.InvokeVoidAsync("ferryLoadingFunctions.animateLoading");
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task StartGameAsync(string messageContents)
        {
            if (ShouldRenderForTerminalState)
            {
                await UpdateTerminalState(Enums.TerminalStates.FerryToHelsinkiGameplay);
            }

            Console.WriteLine(messageContents);
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}

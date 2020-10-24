using FerryToHelsinkiWebsite.Data.Models;
using FerryToHelsinkiWebsite.Data.Repositories;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FerryToHelsinki.Pages
{
    public partial class FetchData 
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private MessageRepository MessageRepository { get; set; }

        private List<Message> Messages { get; set; }

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/messagehub"))
                .Build();

            hubConnection.On<string, string, string>("SendMessage", (title, user, messageContents) =>
            {
                Messages.Add(new Message
                {
                    UserName = user,
                    MessageContents = messageContents
                });

                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetData();
                await InvokeAsync(StateHasChanged);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task GetData()
        {
            Messages = await MessageRepository.GetMessagesAsync();
        }
    }
}

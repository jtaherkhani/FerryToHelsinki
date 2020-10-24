using FerryToHelsinki.Data;
using FerryToHelsinkiWebsite.Data.Models;
using FerryToHelsinkiWebsite.Data.Repositories;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages
{
    public partial class MessageEntry
    {
        [Inject]
        private MessageRepository MessageRepository { get; set; }

        [Inject]
        private MessageClient MessageClient { get; set; }

        private Message Message = new Message();

        private async Task HandleValidSubmit()
        {
            await MessageClient.CreateMessage(Message);
            //await MessageRepository.CreateMessageAsync(Message);
            Message = new Message();
        }
    }
}

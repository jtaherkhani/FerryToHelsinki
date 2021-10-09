using FerryToHelsinki.Data;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages
{
    public partial class MessageEntry
    {

        [Inject]
        private MessageClient MessageClient { get; set; }

        private Message Message = new();

        public async Task HandleValidSubmit()
        {
            await MessageClient.CreateMessage(Message);
            Message = new Message();
        }
    }
}

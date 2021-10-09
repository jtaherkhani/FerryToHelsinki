using FerryToHelsinki.Hubs;
using FerryToHelsinkiWebsite.Data.Models;
using FerryToHelsinkiWebsite.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace FerryToHelsinki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MessageHub, IMessageClient> _messageHubContext;

        public MessagesController(IHubContext<MessageHub, IMessageClient> hubContext)
        {
            _messageHubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(Message message)
        {
            await _messageHubContext.Clients.All.SendMessage(message.UserName, message.MessageContents);
            return Ok();
        }
    }
}

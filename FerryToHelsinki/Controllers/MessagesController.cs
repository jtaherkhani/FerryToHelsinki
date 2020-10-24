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
        private readonly MessageRepository _messageRepository;
        private readonly IHubContext<MessageHub, IMessageClient> _messageHubContext;

        public MessagesController(FerryToHelsinkiDBContext dbContext, IHubContext<MessageHub, IMessageClient> hubContext)
        {
            _messageRepository = new MessageRepository(dbContext);
            _messageHubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(Message message)
        {
            await _messageRepository.CreateMessageAsync(message);
            await _messageHubContext.Clients.All.SendMessage(message.UserName, message.MessageContents);
            return Ok();
        }
    }
}

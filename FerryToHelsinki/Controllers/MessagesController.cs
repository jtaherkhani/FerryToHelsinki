using FerryToHelsinki.Hubs;
using FerryToHelsinki.Singleton;
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
        private readonly AcceptMessagesSingleton _acceptMessagesSingleton;

        public MessagesController(IHubContext<MessageHub, IMessageClient> hubContext, AcceptMessagesSingleton acceptMessagesSingleton)
        {
            _messageHubContext = hubContext;
            _acceptMessagesSingleton = acceptMessagesSingleton;
        }

        [HttpGet]
        public ActionResult GetMessageStatus()
        {
            return Ok(_acceptMessagesSingleton.AcceptMessages);
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(Message message)
        {
            if (!_acceptMessagesSingleton.AcceptMessages)
            {
                return BadRequest();
            }

            await _messageHubContext.Clients.All.SendMessage(message.UserName, message.MessageContents, message.ImageUrl);
            _acceptMessagesSingleton.AcceptMessages = false;
            return Ok();
        }
    }
}

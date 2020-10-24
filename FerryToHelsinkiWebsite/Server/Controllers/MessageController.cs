using FerryToHelsinkiWebsite.Data.Models;
using FerryToHelsinkiWebsite.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FerryToHelsinkiWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageRepository _repository;

        public MessageController(FerryToHelsinkiDBContext dBContext)
        {
            _repository = new MessageRepository(dBContext);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Message message)
        {
            await _repository.CreateMessageAsync(message);
            return Ok();
        }
    }
}

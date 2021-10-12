using FerryToHelsinki.Singleton;
using Microsoft.AspNetCore.Mvc;

namespace FerryToHelsinki.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GameStateController : ControllerBase
    {
        private readonly GameStateSingleton _gameStateSingleton;

        public GameStateController(GameStateSingleton gameStateSingleton)
        {
            _gameStateSingleton = gameStateSingleton;
        }

        [HttpGet]
        public ActionResult GetMessageStatus()
        {
            return Ok(_gameStateSingleton.GameStarted);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDBGames.Model;
using MongoDBGames.Repository;

namespace MongoDBGames.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameRepository gameRepository; // Dependency injection is done using this interface (1)
        public GamesController(IGameRepository _gameRepository) { gameRepository = _gameRepository; }

        #region // Routes -------------------------------------------------------------------------
        // GET: api/Game
        [HttpGet]
        public async Task<IActionResult> Get() { return new ObjectResult(await gameRepository.GetAllGames()); }

        // GET: api/Game/name
        [HttpGet("{name}", Name = "Get")]
        public async Task<IActionResult> Get(string name)
        {
            var game = await gameRepository.GetGame(name);
            if (game == null) return new NotFoundResult();
            return new ObjectResult(game);
        }

        // POST: api/Game
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Game game)
        {
            await gameRepository.Create(game);
            return new OkObjectResult(game);
        }

        // PUT: api/Game/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody]Game game)
        {
            var gameFromDb = await gameRepository.GetGame(name);
            if (gameFromDb == null) return new NotFoundResult();
            game.Id = gameFromDb.Id;
            await gameRepository.Update(game);
            return new OkObjectResult(game);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var gameFromDb = await gameRepository.GetGame(name);
            if (gameFromDb == null) return new NotFoundResult();
            await gameRepository.Delete(name);
            return new OkResult();
        }
        #endregion --------------------------------------------------------------------------------
    }
}

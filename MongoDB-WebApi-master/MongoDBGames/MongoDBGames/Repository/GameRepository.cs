using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDBGames.Model;

namespace MongoDBGames.Repository
{
    public interface IGameRepository // Interface defined here as CRUD operation to be used in controller (2)
    {
        Task Create(Game game);
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game> GetGame(string name);
        Task<bool> Update(Game game);
        Task<bool> Delete(string name);
    }    
    public class GameRepository : IGameRepository // Implementation of the interface is given here (3)
    {
        // Interface to context given at the constructor
        private readonly IGameContext context; // Dependency yo IGameContext (4)
        // Constructor
        public GameRepository(IGameContext _context) { context = _context; }

        #region // Interface IGameRepository is implemented here ----------------------------------
        public async Task Create(Game game) { await context.Games.InsertOneAsync(game); }
        public async Task<IEnumerable<Game>> GetAllGames() { return await context.Games.Find(_ => true).ToListAsync(); }
        public Task<Game> GetGame(string name)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq(m => m.Name, name);
            return context.Games.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<bool> Update(Game game)
        {
            ReplaceOneResult updateResult = await context.Games.ReplaceOneAsync( filter: g => g.Id == game.Id, replacement: game);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string name)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq(m => m.Name, name);
            DeleteResult deleteResult = await context.Games.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        #endregion // -----------------------------------------------------------------------------
    }
}
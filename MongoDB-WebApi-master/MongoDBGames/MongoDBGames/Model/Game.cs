using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDBGames.Model
{
    public class Game
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public List<string> Platforms { get; set; }
    }
    public interface IGameContext // This interface is defined to be used in GameRepository (5)
    { 
        IMongoCollection<Game> Games { get; } 
    }
    public class GameContext : IGameContext // Application of the Game Context is done here (6)
    {
        private readonly IMongoDatabase DB; 
        // IOptions is standard injection, assigned as <Settings> in Startup.cs 
        // IMongoClient is defined as an singleton in Startup.cs
        public GameContext(IOptions<Settings> options, IMongoClient client) { DB = client.GetDatabase(options.Value.Database); }

        // Interface games is implemented here : -------------------------------------------------- (7)
        public IMongoCollection<Game> Games => DB.GetCollection<Game>("Games");  // This is used to access games (8) in the controller
        // ----------------------------------------------------------------------------------------
    }    
}
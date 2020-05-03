// -------------------------------------------------------------------------------
// This is example containing netcore + mongodb
// To run program write         : dotnet run
// To go to the serverd site    : http://localhost:9200/api/games
// Another great approach       : https://www.youtube.com/watch?v=69WBy4MHYUw
// -------------------------------------------------------------------------------

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MongoDBGames
{
    public class Program
    {
        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        public static IWebHost BuildWebHost(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:9200/")
                .Build();
    }
}

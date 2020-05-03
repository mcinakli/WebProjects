using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDBGames.Model;
using MongoDBGames.Repository;

namespace MongoDBGames
{
    public class Startup
    {
        public IConfiguration Configuration { get; }  // Dependency to IConfiguration 
        public Startup(IConfiguration configuration) { Configuration = configuration; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Mongo DB Collection options is read from appsettings.json and set here  
            string SConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
            string SDatabaseName = Configuration.GetSection("MongoDb:Database").Value;
            // Services Configuration is done here, it will be used in GameContext constructor
            services.Configure<Settings>(
                options => // This is the Settings interface given in Settings.cs, it is used in GameContext controller as a dependency
                {
                    options.ConnectionString = SConnectionString;
                    options.Database = SDatabaseName;
                });
            // Services : Interfaces and Class' given here
            // Mongo Client Service defined as Singleton 
            services.AddSingleton<IMongoClient, MongoClient>( _ => new MongoClient(SConnectionString));
            // Game Context & Repository Service defined as Transient : Necessary to be injected in GamesController 
            services.AddTransient<IGameContext, GameContext>();         // This interface uses MongoClient defined above and used in Game Repository below
            services.AddTransient<IGameRepository, GameRepository>();   // This is injected in the GamesController
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // Dependency injection app, env
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseMvc();
        }
    }
}
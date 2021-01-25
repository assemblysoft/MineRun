using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MineRun.Client.Presenters;
using MineRun.Shared;
using MineRun.Shared.Models;

namespace MineRun.Client
{    

    class Program
    {
        static void Main(string[] args)
        {
            //Prepare IConfiguration for DI
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            //ToDo: Register services and options with IServiceCollection


            Console.WriteLine("Welcome to MineRun.\n\nPlease enter your gamer name:");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                var section = Configuration.GetSection("GameData");
                name = section["DefaultPlayerName"];

                if(string.IsNullOrEmpty(name))
                {
                    name = "PlayerX";
                }
            }
            IViewPresenter mineRun = new MineRunConsoleViewPresenter();
            mineRun.Start(name, Level.Level1);            

            Console.WriteLine("\nThanks for playing {0}, come back soon!", name);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //ToDo: Hook this up for console apps
        }
    }
}

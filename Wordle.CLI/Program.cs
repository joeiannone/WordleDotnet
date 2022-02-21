using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordle.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            Command play = new Command("play", "Play a game of Wordle.")
            {
                new Option("--verbose", "Show details"),
            };

            Command stats = new Command("stats", "Display user statistics.")
            {
                new Option("--verbose", "Show details"),
            };

            Command settings = new Command("settings", "Manage settings.")
            {
                new Argument<string>("name", "Setting name."),
                new Option("--on", "Turn on"),
                new Option("--off", "Turn off"),
                new Option(new[] { "--verbose", "-v" }, "Show details."),
            };

            RootCommand cmd = new RootCommand
            {
                play,
                stats,
                settings
            };

            play.Handler = CommandHandler.Create<string, string?, bool, IConsole>(PlayHandler);
            stats.Handler = CommandHandler.Create<string, string?, bool, IConsole>(UserStatsHandler);
            settings.Handler = CommandHandler.Create<string, bool, bool, bool, IConsole>(SettingsHandler);

            return cmd.Invoke(args);
        }

        static void PlayHandler(string name, string? cmd, bool verbose, IConsole console)
        {
            var wf = new WordleFactory();
            var gameController = new GameController(
                        (Game)wf.GetWordleComponent("Game"));
        }

        static void UserStatsHandler(string name, string? cmd, bool verbose, IConsole console)
        {
            var wf = new WordleFactory();
            var gameController = new UserStatsController(
                        (UserStatsService)wf.GetWordleComponent("UserStats"));
        }

        static void SettingsHandler(string name, bool on, bool off, bool verbose, IConsole console)
        {
           
            var wf = new WordleFactory();
            var settingsController = new SettingsController(
                        (SettingsService)wf.GetWordleComponent("Settings"));

            Console.WriteLine(name);
            switch (name)
            {
                case "hardmode":
                    
                    break;
                default:

                    break;
            }
            
        }

        static void DisplayHelpOptions()
        {
            Console.WriteLine("TODO: List commands and options");
        }
    }
}

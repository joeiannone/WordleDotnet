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
            Command play = new Command("play", "Play a game of Wordle.");

            Command stats = new Command("stats", "Display user statistics.");

            Command settings = new Command("settings", "Manage settings.")
            {
                new Argument<string>("settingid", "Available setting id options: \"hard_mode\""),
                new Option<string?>("--set", "Available set options: on, off"),
                new Option(new[] { "--verbose", "-v" }, "Show details."),
            };

            RootCommand cmd = new RootCommand
            {
                play,
                stats,
                settings
            };

            WordleFactory wf = new WordleFactory();

            play.Handler = CommandHandler.Create(
                (IConsole console, Game game) => 
                PlayHandler(console, (Game)wf.GetWordleComponent("Game")));

            stats.Handler = CommandHandler.Create(
                (IConsole console, UserStatsService userStatsService) => 
                UserStatsHandler(console, (UserStatsService)wf.GetWordleComponent("UserStats")));

            settings.Handler = CommandHandler.Create(
                (string settingid, string? set, bool verbose, IConsole console, SettingsService settingsService) => 
                    SettingsHandler(settingid, set, verbose, console, (SettingsService)wf.GetWordleComponent("Settings")));

            return cmd.Invoke(args);
        }

        static void PlayHandler(IConsole console, Game game)
        {
            var gameController = new GameController(game);
        }

        static void UserStatsHandler(IConsole console, UserStatsService userStatsService)
        {
            var userStatsController = new UserStatsController(userStatsService);
        }

        static void SettingsHandler(string settingid, string? set, bool verbose, IConsole console, SettingsService settingsService)
        {
            var settingsController = new SettingsController(settingsService);

            switch (settingid)
            {
                case "hard_mode":
                    if (String.IsNullOrEmpty(set))
                    {
                        settingsController.ShowSetting(settingid);
                    } 
                    else
                    {
                        settingsController.SetSetting(settingid, set);
                        if (verbose)
                            settingsController.ShowSetting(settingid);
                    }
                    break;
            }
            
        }
    }
}

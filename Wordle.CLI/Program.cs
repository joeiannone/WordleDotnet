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

            play.Handler = CommandHandler.Create<bool, IConsole>(PlayHandler);
            stats.Handler = CommandHandler.Create<bool, IConsole>(UserStatsHandler);
            settings.Handler = CommandHandler.Create<string, string?, bool, IConsole>(SettingsHandler);

            return cmd.Invoke(args);
        }

        static void PlayHandler(bool verbose, IConsole console)
        {
            var wf = new WordleFactory();
            var gameController = new GameController((Game) wf.GetWordleComponent("Game"));
        }

        static void UserStatsHandler(bool verbose, IConsole console)
        {
            var wf = new WordleFactory();
            var userStatsController = new UserStatsController((UserStatsService) wf.GetWordleComponent("UserStats"));
        }

        static void SettingsHandler(string settingid, string? set, bool verbose, IConsole console)
        {
           
            var wf = new WordleFactory();
            var settingsController = new SettingsController((SettingsService) wf.GetWordleComponent("Settings"));

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
                    }
                    break;
            }
            
        }
    }
}

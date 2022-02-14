using System;
using System.Collections.Generic;

namespace Wordle.CLI
{
    class Program
    {
        static void Main(string[] args)
        {

            WordleFactory wordleFactory = new WordleFactory();

            string cmd = "";
            List<string> options = new List<string>();

            try
            {
                cmd = args[0].ToLower();

                foreach (string arg in args)
                {
                    // TODO: parse out options if any
                }

            }
            catch (IndexOutOfRangeException)
            {
                DisplayHelpOptions();
                System.Environment.Exit(0);
            }

            switch (cmd)
            {
                case "play":
                    GameController gameController = new GameController(
                        (Game)wordleFactory.GetWordleComponent("Game"));
                    break;
                case "stats":
                    UserStatsController userStatsController = new UserStatsController(
                        (UserStatsService)wordleFactory.GetWordleComponent("UserStats"));
                    break;
                case "settings":
                    SettingsController settingsController = new SettingsController(
                        (SettingsService)wordleFactory.GetWordleComponent("Settings"));
                    break;
                default:
                    Console.WriteLine("Unknown Command");
                    DisplayHelpOptions();
                    break;
            }
            
        }

        static void DisplayHelpOptions()
        {
            Console.WriteLine("TODO: List commands and options");
        }
    }
}

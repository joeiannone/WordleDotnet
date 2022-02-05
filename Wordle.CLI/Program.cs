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
            catch (IndexOutOfRangeException ex)
            {
                DisplayHelpOptions();
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
                default:
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

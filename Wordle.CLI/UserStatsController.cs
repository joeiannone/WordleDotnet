using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;

namespace Wordle.CLI
{
    class UserStatsController
    {
        private Game game;
        private ConsoleColor defaultConsoleForeground = Console.ForegroundColor;

        public UserStatsController()
        {
            game = new Game();
            PrintTopStatsAsync();
        }

        public async void PrintTopStatsAsync()
        {
            List<UserStats> stats = await game.GetUserStatsAsync();
            
            Console.WriteLine("\nUSER STATS:");
            Console.WriteLine("-----  -  ---------------------------");
            foreach (UserStats u in stats)
            {
                if (u.GuessCount <= 3)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (u.GuessCount <= 5)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = defaultConsoleForeground;

                Console.WriteLine($"{u.Word}  {u.GuessCount}  {u.TimeSpan}  {u.StartTime.ToString("d")}");
            }
            Console.WriteLine("");
            Console.ForegroundColor = defaultConsoleForeground;
        }
    }
}

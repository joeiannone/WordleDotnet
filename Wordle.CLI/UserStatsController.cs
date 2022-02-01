using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;

namespace Wordle.CLI
{
    class UserStatsController
    {
        private Game game;

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
                if (u.GuessCount <= 4)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{u.Word}  {u.GuessCount}  {u.TimeSpan}  {u.StartTime.ToString("d")}");
            }
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

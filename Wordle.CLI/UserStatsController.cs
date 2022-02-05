using System;
using System.Collections.Generic;
using Wordle.Models;

namespace Wordle.CLI
{
    class UserStatsController
    {
        private UserStatsService userStatsService;
        private ConsoleColor defaultConsoleForeground = Console.ForegroundColor;

        public UserStatsController(UserStatsService userStats)
        {
            userStatsService = userStats;
            PrintTopStatsAsync();
        }

        public async void PrintTopStatsAsync()
        {
            List<UserStats> stats = await userStatsService.GetUserStatsAsync();
            int gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            double winPercentage = await userStatsService.GetWinPercentageAsync();
            
            Console.WriteLine("\nUSER STATS:");

            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Games played: {gamesPlayed}");
            Console.WriteLine($"Win percentage: {winPercentage}%");
            Console.WriteLine("-------------------------------------");
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

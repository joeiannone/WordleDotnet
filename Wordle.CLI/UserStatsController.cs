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
            List<UserStats> stats = await userStatsService.GetUserStatsAsync(10);
            int gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            double winPercentage = await userStatsService.GetWinPercentageAsync();
            int currentStreak = await userStatsService.GetCurrentStreakAsync();
            int maxStreak = await userStatsService.GetMaxStreakAsync();
            Dictionary<int, int> guessDistribution = await userStatsService.GetGuessDistributionAsync();
         

            Console.WriteLine("\nSTATISTICS:");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Games Played:       {gamesPlayed}");
            Console.WriteLine($"Win %:              {winPercentage}%");
            Console.WriteLine($"Current Streak:     {currentStreak}");
            Console.WriteLine($"Max Streak:         {maxStreak}");
            Console.WriteLine();
            Console.WriteLine("GUESS DISTRIBUTION:");
            Console.WriteLine("--------------------------------------");
            // print in order of key
            for (int i = 1; i < guessDistribution.Count + 1; i++)
            {
                if (guessDistribution.ContainsKey(i))
                {
                    Console.WriteLine($"{i} -----> {guessDistribution[i]}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("TOP 10");
            Console.WriteLine("--------------------------------------");
            foreach (UserStats u in stats)
            {
                /*
                if (u.GuessCount <= 3)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (u.GuessCount <= 5)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = defaultConsoleForeground;
                */

                Console.WriteLine($"{u.Word}  {u.GuessCount}  {u.TimeSpan}  {u.StartTime.ToString("yyyy-MM-dd")}");
            }
            Console.WriteLine("");
            Console.ForegroundColor = defaultConsoleForeground;
        }
    }
}

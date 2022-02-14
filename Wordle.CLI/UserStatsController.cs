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
            PrintStats();
        }

        public void PrintStats()
        {
            Console.WriteLine("\nSTATISTICS:");
            Console.WriteLine("--------------------------------------");
            int gamesPlayed = userStatsService.GetGamesPlayed();
            Console.WriteLine($"Games Played:       {gamesPlayed}");
            double winPercentage = userStatsService.GetWinPercentage();
            Console.WriteLine($"Win %:              {winPercentage}%");
            int currentStreak = userStatsService.GetCurrentStreak();
            Console.WriteLine($"Current Streak:     {currentStreak}");
            int maxStreak = userStatsService.GetMaxStreak();
            Console.WriteLine($"Max Streak:         {maxStreak}");
            Console.WriteLine();
            Console.WriteLine("GUESS DISTRIBUTION:");
            Console.WriteLine("--------------------------------------");
            Dictionary<int, int> guessDistribution = userStatsService.GetGuessDistribution();
            // print in order of key
            for (int i = 1; i < guessDistribution.Count + 1; i++)
            {
                if (guessDistribution.ContainsKey(i))
                {
                    Console.WriteLine($"{i} -----> {guessDistribution[i]}");
                }
            }

            List<UserStats> stats = userStatsService.GetUserStats(10);
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

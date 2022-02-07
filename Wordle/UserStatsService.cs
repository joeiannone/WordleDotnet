using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wordle.Interfaces;
using Wordle.Models;

namespace Wordle
{
    public class UserStatsService : IWordleComponent
    {
        private string DBConnectionString;

        public UserStatsService(string dbConnectionString)
        {
            DBConnectionString = dbConnectionString;
        }

        public Task<int> GetGamesPlayedAsync()
        {
            int count;
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                count = connection.Table<UserStats>().Count();
            }
            return Task.FromResult(count);
        }

        public Task<double> GetWinPercentageAsync()
        {
            double percentage;
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                double wins = connection.Table<UserStats>().Where(t => t.SolutionFound == true).Count();
                double total = connection.Table<UserStats>().Count();
                percentage = Math.Round(wins / total * 100, 2);
            }
            return Task.FromResult(percentage);
        }

        public Task<int> GetCurrentStreakAsync()
        {
            int streak = 0;
            List<UserStats> userStatsList = GetAllUserStats();
            userStatsList.Reverse();
            foreach (UserStats s in userStatsList)
            {
                if (!s.SolutionFound)
                {
                    break;
                }
                else
                {
                    streak++;
                }
            }
            return Task.FromResult(streak);
        }

        public Task<int> GetMaxStreakAsync()
        {
            int streak = 0;
            int max = 0;
            List<UserStats> userStatsList = GetAllUserStats();
            foreach (UserStats s in userStatsList)
            {
                
                if (!s.SolutionFound)
                {
                    if (streak > max)
                        max = streak;
                    
                    streak = 0;
                }
                else
                {
                    streak++;
                }
            }

            if (streak > max)
                max = streak;
            
            return Task.FromResult(max);
        }

        public Task<Dictionary<int, int>> GetGuessDistributionAsync()
        {
            Dictionary<int, int> guessDistribution = new Dictionary<int, int>();

            var dist = from userStats in GetAllUserStats()
                       where userStats.SolutionFound == true
                       orderby userStats.GuessCount
                       group userStats by userStats.GuessCount;

            foreach (var d in dist)
            {
                guessDistribution.Add(d.Key, d.Count());
            }

            /**
             * Need to fill in empty distributions
             * Problem: Don't want to assume a fixed number of rows but want to 
             * show all possible guess counts less than the max in the database
             * Solution: From max distribution key decrement and check if 
             * the next less key is set, if not then initialize to zero. 
             * Not a performance concern because we CAN assume n will 
             * always be a relatively small number
             */
            for (int i = guessDistribution.Keys.Max(); i > 0; i--)
            {
                if (!guessDistribution.ContainsKey(i))
                    guessDistribution.Add(i, 0);
            }

            return Task.FromResult(guessDistribution);
        }

        /**
         * @param dval is the distribution value
         * @param is the max distribution value in the set
         */
        public static double GetDistributionPercentage(double dval, double max)
        {
            return (dval / max) * 100;
        }

        private List<UserStats> GetAllUserStats()
        {
            List<UserStats> userStatsList = new List<UserStats>();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                userStatsList = connection.Table<UserStats>().ToList();
            }
            return userStatsList;
        }

        public Task<List<UserStats>> GetUserStatsAsync(int top = 20)
        {
            UserStats[] userStatsArray = new UserStats[top];
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                userStatsArray = connection.Query<UserStats>(
                    $"SELECT * FROM UserStats WHERE SolutionFound = true ORDER BY GuessCount, TimeSpan LIMIT {top}").ToArray();
            }
            return Task.FromResult(new List<UserStats>(userStatsArray));
        }


    }
}

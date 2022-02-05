using SQLite;
using System;
using System.Collections.Generic;
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
            double percentage = 0;
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                double wins = connection.Table<UserStats>().Where(t => t.SolutionFound == true).Count();
                double total = connection.Table<UserStats>().Count();
                percentage = wins / total * 100;
            }
            return Task.FromResult(percentage);
        }

        public int GetCurrentStreak()
        {
            return 1;
        }

        public int GetMaxStreak()
        {
            return 1;
        }

        public Task<List<UserStats>> GetUserStatsAsync(int top = 20)
        {
            List<UserStats> userStatsList = new List<UserStats>();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                userStatsList = connection.Table<UserStats>().Where(t => t.SolutionFound == true).OrderByDescending(t => t.GuessCount).OrderByDescending(t => t.TimeSpan).Take(top).ToList();
            }
            userStatsList.Reverse();
            return Task.FromResult(userStatsList);
        }


    }
}

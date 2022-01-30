using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    public class UserStats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Word { get; set; }
        public int GuessCount { get; set; }
        public Boolean SolutionFound { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TimeSpan { get; set; }

        public static UserStats CreateUserStatsModel(string word)
        {
            return new UserStats()
            {
                Word = word,
                StartTime = DateTime.Now
            };
        }
    }
}

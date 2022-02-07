using NUnit.Framework;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models;

namespace Wordle.Test
{
    [TestFixture]
    class UserStatsServiceUnitTests
    {

        UserStatsService userStatsService;
        private string AppDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle/Data/Test");
        private string DBConnectionString;

        [SetUp]
        public void SetUp()
        {
            WordleFactory wordle = new WordleFactory(AppDataFolderPath);
            DBConnectionString = Path.Combine(AppDataFolderPath, "Wordle.db");
            userStatsService = (UserStatsService)wordle.GetWordleComponent("UserStats");
        }

        [Test]
        public async Task GetGamesPlayedTest()
        {
            ClearTable();

            int gamesPlayed;

            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(0, gamesPlayed);

            // setup test data
            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 1, SolutionFound = true });
            ClearTable();
            Insert(userStatsList);
            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(4, gamesPlayed);
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 3, SolutionFound = true });
            ClearTable();
            Insert(userStatsList);
            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(9, gamesPlayed);

        }

        [Test]
        public async Task GetMaxStreakTest()
        {
            ClearTable();

            int maxStreak;

            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(0, maxStreak);

            // setup test data
            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 2, SolutionFound = true });
            ClearTable();
            Insert(userStatsList);
            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(2, maxStreak);
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 3, SolutionFound = true });
            ClearTable();
            Insert(userStatsList);
            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(4, maxStreak);
        }

        private void ClearTable()
        {
            // setup test data
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.DropTable<UserStats>();
                connection.CreateTable<UserStats>();
            }
        }

        private void Insert(List<UserStats> userStatsList)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.InsertAll(userStatsList);
            }
        }
    }
}

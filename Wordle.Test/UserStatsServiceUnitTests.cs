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
            ResetTable();

            int gamesPlayed;

            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(0, gamesPlayed);

            // setup test data
            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 1, SolutionFound = true });
            ResetTable(userStatsList);
            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(4, gamesPlayed);
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 3, SolutionFound = true });
            ResetTable(userStatsList);
            gamesPlayed = await userStatsService.GetGamesPlayedAsync();
            Assert.AreEqual(9, gamesPlayed);

        }

        [Test]
        public async Task GetMaxStreakTest()
        {
            ResetTable();

            int maxStreak;

            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(0, maxStreak);

            // setup test data
            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 2, SolutionFound = true });
            ResetTable(userStatsList);
            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(2, maxStreak);
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 3, SolutionFound = true });
            ResetTable(userStatsList);
            maxStreak = await userStatsService.GetMaxStreakAsync();
            Assert.AreEqual(4, maxStreak);
        }

        [Test]
        public async Task GetCurrentStreakTest()
        {
            ResetTable();
            int currentStreak;
            currentStreak = await userStatsService.GetCurrentStreakAsync();
            Assert.AreEqual(0, currentStreak);

            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 6, SolutionFound = false });
            ResetTable(userStatsList);
            currentStreak = await userStatsService.GetCurrentStreakAsync();
            Assert.AreEqual(0, currentStreak);

            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 6, SolutionFound = true });
            ResetTable(userStatsList);
            currentStreak = await userStatsService.GetCurrentStreakAsync();
            Assert.AreEqual(1, currentStreak);

            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 6, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 6, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 6, SolutionFound = true });
            ResetTable(userStatsList);
            currentStreak = await userStatsService.GetCurrentStreakAsync();
            Assert.AreEqual(3, currentStreak);

            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 6, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test10", GuessCount = 6, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test11", GuessCount = 6, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test12", GuessCount = 6, SolutionFound = false });
            ResetTable(userStatsList);
            currentStreak = await userStatsService.GetCurrentStreakAsync();
            Assert.AreEqual(0, currentStreak);

        }

        [Test]
        public async Task GetWinPercentageTest()
        {
            ResetTable();
            double winPercentage;
            winPercentage = await userStatsService.GetWinPercentageAsync();
            Assert.AreEqual(0, winPercentage);
            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 6, SolutionFound = false });
            ResetTable(userStatsList);
            winPercentage = await userStatsService.GetWinPercentageAsync();
            Assert.AreEqual(Math.Round(1.0 / 2.0 * 100, 2), winPercentage);

            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 1, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test8", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test9", GuessCount = 3, SolutionFound = true });

            ResetTable(userStatsList);
            winPercentage = await userStatsService.GetWinPercentageAsync();
            Assert.AreEqual(Math.Round(7.0 / 11.0 * 100, 2), winPercentage);

        }

        [Test]
        public async Task GetGuessDistributionTest()
        {
            ResetTable();
            Dictionary<int, int> guessDistribution;
            guessDistribution = await userStatsService.GetGuessDistributionAsync();
            Assert.AreEqual(new Dictionary<int, int> {{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 } }, guessDistribution);

            List<UserStats> userStatsList = new List<UserStats>();
            userStatsList.Add(new UserStats() { Word = "test1", GuessCount = 3, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test2", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test3", GuessCount = 2, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test4", GuessCount = 4, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test5", GuessCount = 6, SolutionFound = false });
            userStatsList.Add(new UserStats() { Word = "test6", GuessCount = 4, SolutionFound = true });
            userStatsList.Add(new UserStats() { Word = "test7", GuessCount = 3, SolutionFound = false });
            ResetTable(userStatsList);
            guessDistribution = await userStatsService.GetGuessDistributionAsync();
            Assert.AreEqual(new Dictionary<int, int> { { 1, 0 }, { 2, 1 }, { 3, 1 }, { 4, 2 }, { 5, 0 }, { 6, 0 } }, guessDistribution);
        }

        private void ResetTable(List<UserStats> userStatsList = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.DropTable<UserStats>();
                connection.CreateTable<UserStats>();
                if (userStatsList != null)
                    connection.InsertAll(userStatsList);
            }
        }
    }
}

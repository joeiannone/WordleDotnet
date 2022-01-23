using System;
using Wordle;
using NUnit.Framework;

namespace WordleTests
{
    [TestFixture]
    public class GameUnitTest
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            // create sqlite db string
            string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data");
            string DBPath = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            _game = new Game(DBPath);
        }

        [Test]
        public void TestGuess()
        {
            // TODO
            Assert.AreEqual(true, true);
        }
    }
}
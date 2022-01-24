using System;
using System.Collections.Generic;
using Wordle;
using Wordle.Models;
using NUnit.Framework;

namespace WordleTests
{
    [TestFixture]
    public class GameUnitTest
    {
        private Game game;

        [SetUp]
        public void SetUp()
        {
            // create sqlite db string
            string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data");
            string DBPath = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            game = new Game(DBPath);
            
        }

        [Test]
        public void TestGuess()
        {
            game.CurrentSecretWord = Word.CreateWord("chair");
            
            Word guess = game.Guess("couch");
            Dictionary<string, Word.LetterState> correctState = new Dictionary<string, Word.LetterState>();
            correctState.Add("00", Word.LetterState.isCorrect);
            correctState.Add("01", Word.LetterState.notInWord);
            correctState.Add("02", Word.LetterState.notInWord);
            correctState.Add("03", Word.LetterState.notInWord);
            correctState.Add("04", Word.LetterState.inWord);
            Assert.AreEqual(correctState, guess.LetterStates);
            
        }
    }
}
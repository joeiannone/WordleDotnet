using System;
using System.Collections.Generic;
using Wordle;
using Wordle.Models;
using NUnit.Framework;
using static System.Net.Mime.MediaTypeNames;
using SQLite;

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
            string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data\\Test");
            string DBPath = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            using (SQLiteConnection connection = new SQLiteConnection(DBPath))
            {
                connection.DropTable<Word>();
                connection.CreateTable<Word>();
                connection.Insert(Word.CreateWord("teach"));
                connection.Insert(Word.CreateWord("irony"));
                connection.Insert(Word.CreateWord("react"));

            }

            game = new Game(DBPath);

        }

        [Test]
        public void TestGuess()
        {
            game.CurrentSecretWord = Word.CreateWord("teeth");

            ValidatedWord guess = game.ValidateWord("teach");
            
            ValidatedWord guessResult = game.Guess(guess);

            Dictionary<string, Word.LetterState> correctState = new Dictionary<string, Word.LetterState>();
            correctState.Add("00", Word.LetterState.isCorrect);
            correctState.Add("01", Word.LetterState.isCorrect);
            correctState.Add("02", Word.LetterState.notInWord);
            correctState.Add("03", Word.LetterState.notInWord);
            correctState.Add("04", Word.LetterState.isCorrect);
            Assert.AreEqual(correctState, guess.LetterStates);
        }

        [Test]
        public void TestWordValidation()
        {
            ValidatedWord word;

            word = game.ValidateWord("teach");
            Assert.AreEqual(word.IsValid, true);
            Assert.AreEqual(word.ValidationMessages.Count, 0);

            word = game.ValidateWord("sdfsdfs");
            Assert.AreEqual(word.IsValid, false);
            Assert.AreEqual(word.ValidationMessages.Count, 1);

            word = game.ValidateWord("gfdsh");
            Assert.AreEqual(word.IsValid, false);
            Assert.AreEqual(word.ValidationMessages.Count, 1);

        }
    }
}
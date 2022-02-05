using System;
using System.Collections.Generic;
using Wordle;
using Wordle.Models;
using NUnit.Framework;
using SQLite;
using System.Threading.Tasks;

namespace WordleTests
{
    [TestFixture]
    public class GameUnitTest
    {
        private Game game;
        private UserStatsService userStats;

        [SetUp]
        public void SetUp()
        {
            WordleFactory wordle = new WordleFactory();
            game = (Game)wordle.GetWordleComponent("Game");
            userStats = (UserStatsService)wordle.GetWordleComponent("UserStats");
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
        public void TestGuess2()
        {
            game.CurrentSecretWord = Word.CreateWord("dealt");

            ValidatedWord guess = game.ValidateWord("teeth");

            ValidatedWord guessResult = game.Guess(guess);

            //Console.WriteLine(game.CurrentSecretWord);
            //Console.WriteLine(guessResult.ToString());
            //Console.WriteLine(guessResult.LetterStatesToString());

            Dictionary<string, Word.LetterState> correctState = new Dictionary<string, Word.LetterState>();
            correctState.Add("00", Word.LetterState.inWord);
            correctState.Add("01", Word.LetterState.isCorrect);
            correctState.Add("02", Word.LetterState.notInWord);
            correctState.Add("03", Word.LetterState.inWord);
            correctState.Add("04", Word.LetterState.notInWord);
            Assert.AreEqual(correctState, guess.LetterStates);

            
        }

        [Test]
        public void TestGuess3()
        {
            game.CurrentSecretWord = Word.CreateWord("zingy");

            ValidatedWord guess = game.ValidateWord("biggy");

            ValidatedWord guessResult = game.Guess(guess);

            Dictionary<string, Word.LetterState> correctState = new Dictionary<string, Word.LetterState>();
            correctState.Add("00", Word.LetterState.notInWord);
            correctState.Add("01", Word.LetterState.isCorrect);
            correctState.Add("02", Word.LetterState.notInWord);
            correctState.Add("03", Word.LetterState.isCorrect);
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

            try
            {
                word = game.ValidateWord("sdfsdfs");
                Assert.AreEqual(word.IsValid, false);
                Assert.AreEqual(word.ValidationMessages.Count, 1);
            }
            catch (InvalidOperationException ex)
            {
                
                Assert.AreEqual(ex.Message, "Must enter a 5 letter word.");
                
            }
            try
            {
                word = game.ValidateWord("gfdsh");
                Assert.AreEqual(word.IsValid, false);
                Assert.AreEqual(word.ValidationMessages.Count, 1);
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, "Word not found in database.");
            }

            
        }

        [Test]
        public async Task TestGetUserStatsAsync()
        {
            List<UserStats> stats = await userStats.GetUserStatsAsync();
            Console.WriteLine($"Records found: {stats.Count}");
            foreach (UserStats us in stats)
            {
                Console.WriteLine($"{us.Word} - {us.TimeSpan.ToString()} - {us.GuessCount}");
            }
        }

    }
}
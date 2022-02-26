using System;
using System.Collections.Generic;
using Wordle;
using Wordle.Models;
using NUnit.Framework;

namespace Wordle.Test
{
    [TestFixture]
    public class GameUnitTests
    {
        private Game game;
        private SettingsService settingsService;

        [SetUp]
        public void SetUp()
        {
            WordleFactory wordle = new WordleFactory();
            game = (Game)wordle.GetWordleComponent("Game");
            settingsService = (SettingsService)wordle.GetWordleComponent("Settings");
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
        public void HardModeValidationTest()
        {

            // test hard mode validation
            Settings hardmode = settingsService.GetSetting(1, "hard_mode");
            hardmode.BooleanValue = true;
            settingsService.SaveSettings(hardmode);

            if (hardmode.BooleanValue)
            {
                //Must use \"{LettersFound[i]}\" in position {i+1}\n
                //Must use letter \"{l}\"\n
                game.reInit();
                game.CurrentSecretWord = Word.CreateWord("dealt");
                game.Guess(game.ValidateWord("deans"));

                try
                {
                    game.Guess(game.ValidateWord("teeth"));
                }
                catch (InvalidOperationException ex)
                {
                    Assert.AreEqual(ex.Message, "Must use \"d\" in position 1\nMust use \"a\" in position 3\n");
                }

                game.reInit();
                game.CurrentSecretWord = Word.CreateWord("crate");
                game.Guess(game.ValidateWord("aches"));
                try
                {
                    game.Guess(game.ValidateWord("crown"));
                }
                catch (InvalidOperationException ex)
                {
                    Assert.AreEqual(ex.Message, "Must use letter \"a\"\nMust use letter \"e\"\n");
                }

            }
        }

    }
}
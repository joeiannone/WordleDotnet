using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordleWPFClient.Models;
using WordleWPFClient;

namespace WordleWPFClient.Classes
{
    class Game
    {
        public enum Answer {
            Correct,
            Incorrect,
            InvalidWord,

        };
        public string secretWord;
        public int guessLimit;
        public int guessPos { get; set; } = 0;

        public Game(string secretWord, int guessLimit)
        {
            this.secretWord = secretWord;
            this.guessLimit = guessLimit;
        }

        private bool validWord(string wordString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DbConnectionString))
            {
                List<Models.Word> result = connection.Table<Models.Word>().Where(word => word.Text.Equals(wordString)).ToList<Models.Word>();
                
                if (result.Count() > 0)
                    return true;
            }
            return false;
        }

        public Answer validateGuess(string guess)
        {
            if (guess == secretWord)
                return Answer.Correct;

            else if (!validWord(guess))
                return Answer.InvalidWord;

            return Answer.Incorrect;
        }

    }
}

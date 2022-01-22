using SQLite;
using System;
using System.Collections.Generic;
using Wordle.Models;

namespace Wordle
{
    /**
     * Game ViewModel
     */
    public class Game : IDisposable
    {

        public string DBConnectionString;
        public const int COLUMNS = 5;
        public int Rows;
        //public Dictionary<HashCode, Word> CurrentWordStates;
        public List<char> lettersFound;
        public List<char> lettersRemaining;
        private int CurrentRowPosition { get; set; }
        private Word CurrentSecretWord { get; set; }


        public Game(string dbConnectionString, int rows = 6)
        {
            this.DBConnectionString = dbConnectionString; 
            Init(rows);
        }

        private void Init(int rows = 6)
        {
            Rows = rows;
            //CurrentWordStates = new Dictionary<HashCode, Word>();
            lettersFound = new List<char>();
            CurrentSecretWord = Word.CreateWord(GenerateRandomWord());
            lettersRemaining = new List<char>(CurrentSecretWord.Letters);
        }

        public Word Guess(string wordStr)
        {
            //TODO
            /**
             * Possible states of each letter
             * All gray to sdtart
             * 
             * GReen if:
             *  - in word and correct position
             * Yellow if:
             *  - in word but not in correct position and not already green (at any point) - unless the same letter exists somewhere else in the word
             *  
             */

            Word guess = Word.CreateWord(wordStr);

            // determine what letters have been found and update lists
            foreach (char c in guess.Letters)
            {
                if (lettersRemaining.Contains(c))
                {
                    if (Array.IndexOf(guess.Letters, c) == Array.IndexOf(CurrentSecretWord.Letters, c))
                    {
                        lettersFound.Add(c);
                        lettersRemaining.Remove(c);
                    }
                }
            }

            // TODO: figure out letter states for this guess


            return guess;
        }


        private string GenerateRandomWord()
        {
            Word randomWord;

            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                Random rand = new Random();

                int maxId = connection.Table<Word>().Count();
                int randomIndex = rand.Next(maxId);

                List<Word> randomWords = connection.Table<Word>().Where(x => x.Id.Equals(randomIndex)).ToList();
                randomWord = randomWords[0];
            }
            return randomWord.WordStr;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

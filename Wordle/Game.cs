using SQLite;
using System;
using System.Collections.Generic;
using Wordle.Models;

namespace Wordle
{
    /**
     * Game ViewModel
     */
    class Game : IDisposable
    {

        public string DBConnectionString;
        public const int COLUMNS = 5;
        public int Rows;
        public Dictionary<HashCode, Word> CurrentWordStates;
        public char[] lettersFound;
        public char[] lettersRemaining;
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
            CurrentWordStates = new Dictionary<HashCode, Word>();
            lettersFound = new char[COLUMNS];
            lettersRemaining = new char[COLUMNS];
            CurrentSecretWord = GenerateRandomWord();
        }

        public Dictionary<HashCode, Word> Guess(string wordStr)
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
            foreach(char c in guess.Letters)
            {

            }
            return CurrentWordStates;
        }


        private Word GenerateRandomWord()
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
            return randomWord;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

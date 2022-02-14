using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordle.Interfaces;
using Wordle.Models;

namespace Wordle
{
    /**
     * Game ViewModel
     */
    public class Game : IWordleComponent, IDisposable
    {

        private string DBConnectionString;

        public int COLUMNS { get; } = 5;
        public int Rows;
        public DateTime StartTime;
        public TimeSpan timespan { get; set; }

        // this gets changed inside Guess method
        public bool wordFound = false;
        public int CurrentRowPosition { get; set; }
        public Word CurrentSecretWord { get; set; }
        public UserStats GameStats { get; set; }


        public Game(string dbConnectionString, int rows = 6)
        {
            DBConnectionString = dbConnectionString;
            Init(rows);
        }

        /**
         * 
         * 
         */
        private void Init(int rows = 6)
        {
            Rows = rows;
            CurrentSecretWord = Word.CreateWord(GenerateRandomWord());
            StartTime = DateTime.Now;
            GameStats = UserStats.CreateUserStatsModel(CurrentSecretWord.ToString());
        }

        /**
         * 
         */
        public void IncrementRowPosition()
        {
            if (CurrentRowPosition < Rows && !wordFound)
            {
                CurrentRowPosition++;
            }
        }

        /**
         * 
         * 
         */
        public ValidatedWord Guess(ValidatedWord guess)
        {
            if (!guess.IsValid)
            {
                return guess;
            }

            Dictionary<char, int> inWordFrequenciesCountDown = CurrentSecretWord.GetLetterFrequencies();
            List<char> solutionLetters = new List<char>(CurrentSecretWord.Letters);

            // before doing anything check for an exact match
            if (guess.ToString() == CurrentSecretWord.ToString())
            {
                wordFound = true;
                timespan = DateTime.Now - StartTime;
                GameStats.SolutionFound = true;
            }

            //new
            // check for correct first
            for (int i = 0; i < guess.Letters.Length; i++)
            {
                char letter = guess.Letters[i];
                string letterKey = $"{CurrentRowPosition}{i}";

                if (letter == CurrentSecretWord.Letters[i])
                {
                    guess.LetterStates[letterKey] = Word.LetterState.isCorrect;
                    inWordFrequenciesCountDown[letter]--;
                }
            }

            // handle the rest
            for (int i = 0; i < guess.Letters.Length; i++)
            {
                char letter = guess.Letters[i];
                string letterKey = $"{CurrentRowPosition}{i}";

                if (guess.LetterStates.ContainsKey(letterKey))
                    continue;

                if (solutionLetters.Contains(letter) && inWordFrequenciesCountDown[letter] > 0)
                {
                    guess.LetterStates[letterKey] = Word.LetterState.inWord;
                }
                else 
                {
                    guess.LetterStates[letterKey] = Word.LetterState.notInWord;
                }
            }

            if (CurrentRowPosition == Rows - 1 || wordFound)
            {
                GameStats.EndTime = DateTime.Now;
                GameStats.TimeSpan = GameStats.EndTime - GameStats.StartTime;
                GameStats.GuessCount = CurrentRowPosition + 1;
                GameStats.SolutionFound = wordFound;
                SaveGameStats();
            }

            return guess;
        }

        /**
         * 
         * 
         */
        public string GenerateRandomWord()
        {
            Word randomWord;

            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {               
                Random rand = new Random();
                int maxId = connection.Table<Word>().Count();
                int randomIndex = rand.Next(1, maxId);
                randomWord = connection.Table<Word>().Where(x => x.Id.Equals(randomIndex)).FirstOrDefault();
            }
            return randomWord.WordStr;
        }


        private void SaveGameStats()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.Insert(GameStats);
            }
        }


        /**
         * 
         */
        public ValidatedWord ValidateWord(string wordStr)
        {
            wordStr = wordStr.ToLower().Trim();
            string msg;
            Boolean isValid = false;
            List<string> validationMessages = new List<string>();

            if (wordStr.Length != COLUMNS)
            {
                msg = $"Must enter a {COLUMNS} letter word.";
                validationMessages.Add(msg);
                throw new InvalidOperationException(msg);
            } else {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
                {
                    List<Word> wordResult = connection.Table<Word>().Where(x => x.WordStr.Equals(wordStr.ToLower())).ToList();
                    if (wordResult.Count < 1)
                    {
                        msg = $"Word not found in database.";
                        validationMessages.Add(msg);
                        throw new InvalidOperationException(msg);
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }

            return new ValidatedWord()
            {
                WordStr = wordStr,
                Length = wordStr.Length,
                Letters = wordStr.ToCharArray(),
                LetterStates = new Dictionary<string, Word.LetterState>(),
                IsValid = isValid,
                ValidationMessages = validationMessages
            };
        }

        public string GetTimespanDisplayString()
        {
            if (timespan.TotalSeconds >= 3600)
            {
                return string.Format("{0}h {1}m {2}s",
                    timespan.Hours,
                    timespan.Minutes,
                    timespan.Seconds
                );
            }
            else if (timespan.TotalSeconds >= 60)
            {
                return string.Format("{0}m {1}s",
                    timespan.Minutes,
                    timespan.Seconds
                );
            }
            else
            {
                return string.Format("{0}s", timespan.Seconds);
            }
        }

        /**
         * 
         */
        public void Dispose()
        {
            
        }
    }
}

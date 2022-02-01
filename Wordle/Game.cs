using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wordle.Models;

namespace Wordle
{
    /**
     * Game ViewModel
     */
    public class Game : IDisposable
    {
        /**
         * 
         * 
         */
        private static string CurrentDirectory = Environment.CurrentDirectory.ToString();
        private static string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle/Data");
        private static string DBConnectionString = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");

        public int COLUMNS { get; } = 5;
        public int Rows;
        public DateTime StartTime;
        public TimeSpan timespan { get; set; }

        // this gets changed inside Guess method
        public bool wordFound = false;
        public int CurrentRowPosition { get; set; }
        public Word CurrentSecretWord { get; set; }
        public UserStats GameStats { get; set; }


        public Game(int rows = 6)
        {
            Init(rows);
        }

        /**
         * 
         * 
         */
        private void Init(int rows = 6)
        {
            BuildDatabase();
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
                List<Word> randomWords = connection.Table<Word>().Where(x => x.Id.Equals(randomIndex)).ToList();
                randomWord = randomWords[0];
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

        public Task<List<UserStats>> GetUserStatsAsync(int top = 20)
        {
            List<UserStats> userStatsList = new List<UserStats>();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                userStatsList = connection.Table<UserStats>().Where(t => t.SolutionFound == true).OrderByDescending(t => t.GuessCount).OrderByDescending(t => t.TimeSpan).Take(top).ToList();
                //userStatsList = connection.Table<UserStats>().ToList();
            }
            userStatsList.Reverse();
            return Task.FromResult(userStatsList);
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

        public void BuildDatabase()
        {

            // return if file exists and not empty
            if (File.Exists(DBConnectionString))
            {
                FileInfo fileinfo = new FileInfo(DBConnectionString);
                if (fileinfo.Length != 0)
                    return;
            }
            Console.WriteLine("\nCreating file...\n");
            // create app data directory for sqlite file
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            /**
             * Parse words from json file 
             */
            List<Word> Words = new List<Word>();
            using (StreamReader file = File.OpenText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Static/five-letter-words.json")))
            {
                string json = file.ReadToEnd();
                List<string> wordList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                Words = wordList.ConvertAll(wordStr => Word.CreateWord(wordStr));
            }

            /**
             * Create/populate database
             */
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.DropTable<Word>();
                connection.CreateTable<Word>();
                connection.CreateTable<UserStats>();
                connection.InsertAll(Words);
            }
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

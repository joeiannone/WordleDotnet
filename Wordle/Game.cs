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
        /**
         * 
         * 
         */
        public string DBConnectionString;
        public const int COLUMNS = 5;
        public int Rows;
        private List<char> lettersFound;
        private List<char> lettersRemaining;

        // this gets changed inside Guess method
        public bool wordFound = false;
        public int CurrentRowPosition { get; set; }
        public Word CurrentSecretWord { get; set; }


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
            lettersFound = new List<char>();
            CurrentSecretWord = Word.CreateWord(GenerateRandomWord());
        }

        /**
         * 
         */
        public void IncrementRowPosition()
        {
            if (CurrentRowPosition < (Rows - 1) && !wordFound)
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

            lettersRemaining = new List<char>(CurrentSecretWord.Letters);

            // before doing anything check for an exact match
            if (guess.ToString() == CurrentSecretWord.ToString())
            {
                wordFound = true;
            }

            // determine what letters have been found and update lists
            for (int i = 0; i < guess.Letters.Length; i++)
            {
                char letter = guess.Letters[i];

                string letterKey = $"{CurrentRowPosition}{i}";

                if (new List<char>(CurrentSecretWord.Letters).Contains(letter))
                {
                    if (i == Array.IndexOf(CurrentSecretWord.Letters, letter))
                    {
                        lettersFound.Add(letter);
                        lettersRemaining.Remove(letter);
                        guess.LetterStates.Add(letterKey, Word.LetterState.isCorrect);
                    } else
                    {
                        if (lettersRemaining.Contains(letter))
                            guess.LetterStates.Add(letterKey, Word.LetterState.inWord);
                        else
                            guess.LetterStates.Add(letterKey, Word.LetterState.notInWord);
                    }
                }
                else
                {
                    guess.LetterStates.Add(letterKey, Word.LetterState.notInWord);
                }
            }


            lettersFound.Clear();
            lettersRemaining.Clear();

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
                randomWord = (Word) randomWords[0];
            }
            return randomWord.WordStr;
        }


        /**
         * 
         */
        public ValidatedWord ValidateWord(string wordStr)
        {
            Boolean isValid = false;
            List<string> validationMessages = new List<string>();

            if (wordStr.Length != COLUMNS)
            {
                validationMessages.Add( $"Must enter a {COLUMNS} letter word.");
            } else {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
                {
                    List<Word> wordResult = connection.Table<Word>().Where(x => x.WordStr.Equals(wordStr.ToLower())).ToList();
                    if (wordResult.Count < 1)
                    {
                        validationMessages.Add($"Word Not found in database.");
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

        /**
         * 
         */
        public void Dispose()
        {
            
        }
    }
}

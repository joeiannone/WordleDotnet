using SQLite;
using System;
using System.Collections.Generic;
using Wordle.Models;
using static Wordle.Models.Word;

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
        private List<char> lettersFound;
        private List<char> lettersRemaining;
        public int CurrentRowPosition { get; set; }
        public Word CurrentSecretWord { get; set; }

        public Game(string dbConnectionString, int rows = 6)
        {
            this.DBConnectionString = dbConnectionString; 
            Init(rows);
        }

        private void Init(int rows = 6)
        {
            Rows = rows;
            lettersFound = new List<char>();

            try { 
                CurrentSecretWord = Word.CreateWord(GenerateRandomWord());
                lettersRemaining = new List<char>(CurrentSecretWord.Letters);
            } catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
           
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
            for (int i = 0; i < guess.Letters.Length; i++)
            {
                char letter = guess.Letters[i];

                string letterKey = $"{CurrentRowPosition}{i}";

                Console.WriteLine(letterKey);
                if (new List<char>(CurrentSecretWord.Letters).Contains(letter))
                {
                    if (i == Array.IndexOf(CurrentSecretWord.Letters, letter))
                    {
                        lettersFound.Add(letter);
                        lettersRemaining.Remove(letter);
                        guess.LetterStates.Add(letterKey, LetterState.isCorrect);
                    } else
                    {
                        if (lettersRemaining.Contains(letter))
                            guess.LetterStates.Add(letterKey, LetterState.inWord);
                        else
                            guess.LetterStates.Add(letterKey, LetterState.notInWord);
                    }
                }
                else
                {
                    guess.LetterStates.Add(letterKey, LetterState.notInWord);
                }
            }
            
            return guess;
        }


        private string GenerateRandomWord()
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

        public void Dispose()
        {
            
        }
    }
}

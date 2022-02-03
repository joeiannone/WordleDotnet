using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;

namespace Wordle.CLI
{
    class GameController
    {
        private Game game;
        private List<ValidatedWord> Guesses;
        private ConsoleColor defaultConsoleForeground = Console.ForegroundColor;

        public GameController(int rows = 6)
        {
            Init(rows);
        }

        private void Init(int rows)
        {
            game = new Game(rows);
            Guesses = new List<ValidatedWord>();
            Prompt();
        }

        private void SubmitGuess(string guessStr)
        {
            if (game.wordFound || game.CurrentRowPosition == game.COLUMNS + 1)
                return;

            try
            {
                ValidatedWord guess = game.ValidateWord(guessStr);
                ValidatedWord guessResult = game.Guess(guess);
                Guesses.Add(guessResult);

                Console.WriteLine("");
                int row = 0;
                foreach (ValidatedWord g in Guesses)
                {
                    int col = 0;
                    foreach (char c in g.ToString().ToCharArray())
                    {
                        switch (g.LetterStates[$"{row}{col}"])
                        {
                            case Word.LetterState.inWord:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case Word.LetterState.notInWord:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case Word.LetterState.isCorrect:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                        }
                        Console.Write($"{g.ToString().ToCharArray()[col].ToString().ToUpper()} ");
                        col++;
                    }
                    row++;
                    Console.WriteLine("");
                }
                Console.WriteLine("");
                game.IncrementRowPosition();

            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{ex.Message}\nTry again.\n");
            }

            Console.ForegroundColor = defaultConsoleForeground;


        }

        private void Prompt()
        {
            string guess;
            Console.WriteLine("\nBegin:\n");

            while (game.CurrentRowPosition < game.Rows)
            {
                if (game.wordFound)
                    break;

                guess = Console.ReadLine();

                SubmitGuess(guess);
                
            }

            if (game.wordFound)
            {
                Console.WriteLine($"\nCongrats you got it!\n");
            }
            else
            {
                Console.WriteLine($"\nBetter luck next time.\n\nThe word was {game.CurrentSecretWord.ToString()}...\n");
            }
            


        }
    }
}

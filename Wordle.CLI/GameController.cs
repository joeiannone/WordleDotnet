using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;

namespace Wordle.CLI
{
    class GameController
    {
        private Game game;

        public GameController(int rows = 6)
        {
            Init(rows);
        }

        private void Init(int rows)
        {
            game = new Game(rows);
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
                Console.WriteLine($"\n{guessResult.LetterStatesToString()}");
                game.IncrementRowPosition();

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }


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

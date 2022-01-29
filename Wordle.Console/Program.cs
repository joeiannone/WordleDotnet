using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using Wordle;
using Wordle.Models;
using static System.Net.Mime.MediaTypeNames;
using static Wordle.Models.Word;

namespace WordleConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Game game = new Game(6);
            Console.WriteLine(game.CurrentSecretWord);
            ValidatedWord guess = game.ValidateWord("tasks");
            ValidatedWord guessResult = game.Guess(guess);

            foreach (KeyValuePair<string, LetterState> kvp in guessResult.LetterStates)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}

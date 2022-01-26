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
            
            // create sqlite db string
            string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data");
            string DBPath = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            List<string> wordStringList = JArray.Parse("[\"beach\", \"chair\", \"tears\", \"teach\", \"beers\"]").ToObject<List<string>>();
            List<Word> Words = wordStringList.ConvertAll(wordString => (new Word() { WordStr = wordString }));
            //Words.Clear();
            //Words.Add(new Word() { WordStr = "tears" });
            using (SQLiteConnection connection = new SQLiteConnection(DBPath))
            {
                connection.DropTable<Word>();
                connection.CreateTable<Word>();
                connection.InsertAll(Words);
            }

            Game game = new Game(DBPath);
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

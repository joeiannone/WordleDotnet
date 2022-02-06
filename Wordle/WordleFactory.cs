using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Wordle.Interfaces;
using Wordle.Models;

namespace Wordle
{
    /**
     * Factory class for all components involved in Wordle so we can easily inject the DB variables
     * into the respective classes.
     * 
     */
    public class WordleFactory
    {
        private static string AppDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle/Data");
        private static string DBConnectionString = Path.Combine(AppDataFolderPath, "Wordle.db");

        public WordleFactory()
        {
            BuildDatabase();
        }

        public IWordleComponent GetWordleComponent(string type)
        {
            type = type.ToLower();

            switch (type)
            {
                case "game":
                    return new Game(DBConnectionString);

                case "userstats":
                    return new UserStatsService(DBConnectionString);

                case "settings":
                    return new SettingsService(DBConnectionString);

                default:
                    throw new ArgumentException("Invalid type");
            }
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
    }
}

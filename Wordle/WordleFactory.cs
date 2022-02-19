using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Wordle.Interfaces;
using Wordle.Models;
using static Wordle.Models.Settings;

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

        public WordleFactory(string appDataFolderPath = null)
        {
            // allow optional override
            if (appDataFolderPath != null)
            {
                AppDataFolderPath = appDataFolderPath;
            }
            DBConnectionString = Path.Combine(AppDataFolderPath, "Wordle.db");

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

        public void BuildDatabase(Boolean force = false)
        {

            // return if file exists and not empty
            if (File.Exists(DBConnectionString))
            {
                FileInfo fileinfo = new FileInfo(DBConnectionString);
                if (fileinfo.Length != 0 && !force)
                    return;
            }

            // create app data directory for sqlite file
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            // set up words
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

            // create settings form fields
            List<Settings> settingsList = new List<Settings>();
            // hard mode field
            Settings hardMode = Settings.CreateSettingsModel(1, "hard_mode", FormFieldType.Boolean);
            hardMode.BooleanValue = false;
            settingsList.Add(hardMode);
            // dark mode field
            Settings darkMode = Settings.CreateSettingsModel(1, "dark_mode", FormFieldType.Boolean);
            darkMode.BooleanValue = false;
            settingsList.Add(darkMode);
            // high contrast mode
            Settings highContrastMode = Settings.CreateSettingsModel(1, "high_contrast_mode", FormFieldType.Boolean);
            highContrastMode.BooleanValue = false;
            settingsList.Add(highContrastMode);

            /**
             * Create/populate tables
             */
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                // words
                connection.DropTable<Word>();
                connection.CreateTable<Word>();
                connection.InsertAll(Words);

                // settings
                connection.DropTable<Settings>();
                connection.CreateTable<Settings>();
                connection.InsertAll(settingsList);

                // user stats
                connection.CreateTable<UserStats>();
            }
        }
    }
}

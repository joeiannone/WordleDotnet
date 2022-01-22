using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using WordleWPFClient.Models;

namespace WordleWPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static string CurrentDirectory = Environment.CurrentDirectory.ToString();
        //public static string WordsDataPath = System.IO.Path.Combine(CurrentDirectory, "Resources\\five-letter-words.json");

        public static string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data");
        public static string WordsDbPath = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");
        

        public App()
        {

            //create Wordle app data directory
            System.IO.Directory.CreateDirectory(AppDataFolderPath);

            BuildWordsTable();

        }

        private void BuildWordsTable()
        {
            Uri uri = new Uri("/Resources/five-letter-words.json", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            var reader = new JsonTextReader(new StreamReader(info.Stream));
            List<string> wordStringList = JArray.Load(reader).ToObject<List<string>>();
            List<Word> Words = wordStringList.ConvertAll(wordString => (new Word() { Text = wordString }));

            using (SQLiteConnection connection = new SQLiteConnection(App.WordsDbPath))
            {

                connection.Query<Word>("DROP TABLE IF EXISTS Word");
                connection.CreateTable<Word>();
                connection.InsertAll(Words);
            }
        }

    }
}

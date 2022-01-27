using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using Wordle.Models;

namespace Wordle.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static string CurrentDirectory = Environment.CurrentDirectory.ToString();
        //public static string WordsDataPath = System.IO.Path.Combine(CurrentDirectory, "Resources\\five-letter-words.json");
        public static string AppDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle\\Data");
        public static string DbConnectionString = System.IO.Path.Combine(AppDataFolderPath, "Wordle.db");

        public App()
        {
            //create Wordle app data directory
            System.IO.Directory.CreateDirectory(AppDataFolderPath);
            BuildWordsTable();
        }

        private void BuildWordsTable()
        {
            /**
             * populate database
             */
            Uri uri = new Uri("/Resources/five-letter-words.json", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            var reader = new JsonTextReader(new StreamReader(info.Stream));
            List<string> wordStringList = JArray.Load(reader).ToObject<List<string>>();
            List<Word> Words = wordStringList.ConvertAll(wordString => (new Word() { WordStr = wordString, Length = wordString.Length }));

            using (SQLiteConnection connection = new SQLiteConnection(DbConnectionString))
            {
                Console.WriteLine(connection);
                
                connection.DropTable<Word>();
                connection.CreateTable<Word>();
                connection.InsertAll(Words);
            }
        }

    }
}

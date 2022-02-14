using SQLite;
using System.Collections.Generic;
using Wordle.Interfaces;
using Wordle.Models;

namespace Wordle
{
    public class SettingsService : IWordleComponent
    {

        private string DBConnectionString;
        public SettingsService(string dbConnectionString)
        {
            DBConnectionString = dbConnectionString;
        }

        public List<Settings> GetSettingsForm(int formId = 1)
        {
            List<Settings> settingsList = new List<Settings>();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                settingsList = connection.Table<Settings>().Where(x => x.FormId == formId).ToList();
            }
            return settingsList;
        }

        public Settings GetSetting(int settingId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                return connection.Table<Settings>().Where(x => x.Id == settingId).FirstOrDefault();
            }
        }

        public Settings GetSetting(int formId, string name)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                return connection.Table<Settings>().Where(x => x.FormId == formId && x.Name == name).FirstOrDefault();
            }
        }

        public void SaveSettings(Settings _settings)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnectionString))
            {
                connection.Update(_settings);
            }
        }
    }
}

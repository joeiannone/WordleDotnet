using NUnit.Framework;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models;
using static Wordle.Models.Settings;

namespace Wordle.Test
{
    [TestFixture]
    class SettingsServiceUnitTests
    {
        SettingsService settingsService;
        private string AppDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wordle/Data/Test");
        private string DBConnectionString;

        [SetUp]
        public void SetUp()
        {
            DBConnectionString = Path.Combine(AppDataFolderPath, "Wordle.db");
            WordleFactory wordle = new WordleFactory(AppDataFolderPath);
            wordle.BuildDatabase(true);
            settingsService = (SettingsService)wordle.GetWordleComponent("Settings");
        }

        [Test]
        public void AllServiceMethods()
        {

            List<Settings> settingsForm = settingsService.GetSettingsForm(1);
            Assert.AreEqual(3, settingsForm.Count);

            foreach (Settings _settings in settingsForm)
            {
                Settings gotsetting = settingsService.GetSetting(_settings.Id);
                Assert.AreEqual(_settings.FormId, gotsetting.FormId);
                Assert.AreEqual(_settings.Name, gotsetting.Name);
                Assert.AreEqual(_settings.FieldType, gotsetting.FieldType);
                Assert.AreEqual(_settings.IntValue, gotsetting.IntValue);
                Assert.AreEqual(_settings.BooleanValue, gotsetting.BooleanValue);
                Assert.AreEqual(_settings.StringValue, gotsetting.StringValue);
            }

            Random rand = new Random();
            int randindex = rand.Next(settingsForm.Count);

            Settings testsettings = settingsForm[randindex];

            // reusable settings var
            Settings _s;

            switch (testsettings.FieldType)
            {
                case FormFieldType.Boolean:
                    Assert.AreEqual(false, testsettings.BooleanValue);
                    testsettings.BooleanValue = true;
                    settingsService.SaveSettings(testsettings);
                    _s = settingsService.GetSetting(testsettings.FormId, testsettings.Name);
                    Assert.AreEqual(testsettings.Id, _s.Id);
                    Assert.AreEqual(true, _s.BooleanValue);
                    break;
                case FormFieldType.String:
                    break;
                case FormFieldType.Integer:
                    break;
            }

        }


    }
}

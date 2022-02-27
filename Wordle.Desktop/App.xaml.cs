using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            WordleFactory wf = new WordleFactory();
            SettingsService settingsService = (SettingsService)wf.GetWordleComponent("Settings");
            List<Settings> settingsList = settingsService.GetSettingsForm(1);
            Application.Current.Properties["dark_mode"] = false;
            Application.Current.Properties["high_contrast_mode"] = false;
            foreach (Settings s in settingsList)
            {
                switch (s.Name)
                {
                    case "dark_mode":
                        Application.Current.Properties["dark_mode"] = s.BooleanValue;
                        break;
                    case "high_contrast_mode":
                        Application.Current.Properties["high_contrast_mode"] = s.BooleanValue;
                        break;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;
using Wordle.Models;
using static Wordle.Models.Settings;

namespace Wordle.CLI
{
    class SettingsController
    {
        SettingsService settingsService;
        List<Settings> settings;
        int formid = 1;
        Dictionary<string, Boolean> boolValueConversion = new Dictionary<string, bool>
        {
            { "on", true }, { "off", false }
        };

        public SettingsController(SettingsService _settingsService)
        {
            settingsService = _settingsService;
            settings = settingsService.GetSettingsForm();
        }

        public void SetSetting(string settingname, string val)
        {
            // get setting
            try
            {
                Settings s = settingsService.GetSetting(formid, settingname);
                switch (s.FieldType)
                {
                    case FormFieldType.Boolean:
                        s.BooleanValue = boolValueConversion[val];
                        settingsService.SaveSettings(s);
                        break;
                }
            } 
            catch (NullReferenceException)
            {
                Console.WriteLine("Setting not found");
            }
            catch (KeyNotFoundException)
            {

            }
        }

        public void ShowSetting(string settingname)
        {
            // get setting
            try
            {
                Settings s = settingsService.GetSetting(formid, settingname);
                //TODO: get field type before outputting value
                Console.WriteLine($"{s.Name}: {s.BooleanValue.ToString()}");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Setting not found");
            }
            
        }
    }
}

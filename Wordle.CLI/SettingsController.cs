using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;

namespace Wordle.CLI
{
    class SettingsController
    {
        SettingsService settingsService;
        List<Settings> settings;

        public SettingsController(SettingsService _settingsService)
        {
            settingsService = _settingsService;
            settings = settingsService.GetSettingsForm();

            foreach (Settings s in settings)
            {
                Console.WriteLine($"{s.Name} | {s.FormId} | {s.FieldType} | {s.BooleanValue} | {s.IntValue} | {s.StringValue}");
            }
        }
    }
}

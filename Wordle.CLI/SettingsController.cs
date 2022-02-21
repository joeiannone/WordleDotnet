using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
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
        }
    }
}

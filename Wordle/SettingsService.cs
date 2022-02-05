using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Interfaces;

namespace Wordle
{
    class SettingsService : IWordleComponent
    {

        private string DBConnectionString;
        public SettingsService(string dbConnectionString)
        {
            DBConnectionString = dbConnectionString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using Wordle;
using Wordle.Models;
using static System.Net.Mime.MediaTypeNames;
using static Wordle.Models.Word;

namespace Wordle.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string cmd = "";

            try
            {
                cmd = args[0];
            }
            catch (IndexOutOfRangeException ex)
            {

            }

            switch (cmd)
            {
                case "play":
                    GameController gameController = new GameController();
                    break;
                case "stats":
                    UserStatsController userStatsController = new UserStatsController();
                    break;
                default:
                    Console.WriteLine("\nList commands and options\n...\n");
                    break;
            }
            
        }
    }
}

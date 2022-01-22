using SQLite;
using System.Collections.Generic;

namespace WordleWPFClient.Models
{
    class Word
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Text { get; set; }
        public int Length { get; set; }
    }
}

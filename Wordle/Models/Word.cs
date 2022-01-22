using SQLite;

namespace Wordle.Models
{
    public class Word
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string WordStr { get; set; }
        public int Length { get; set; }

        [Ignore]
        public char[] Letters { get; set; }

        public override string ToString()
        {
            return WordStr;
        }

        public static Word CreateWord(string wordStr)
        {
            return new Word {
                WordStr = wordStr,
                Length = wordStr.Length,
                Letters = wordStr.ToCharArray()
            };
        }
    }
}

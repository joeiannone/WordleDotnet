using System;
using System.Collections.Generic;
using SQLite;
using static Wordle.Game;

namespace Wordle.Models
{
    public class Word
    {
        public enum LetterState
        {
            isCorrect,
            inWord,
            notInWord,
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string WordStr { get; set; }

        public int Length { get; set; }

        [Ignore]
        public char[] Letters { get; set; }

        [Ignore]
        public Dictionary<string, LetterState> LetterStates { get; set; }

        public override string ToString()
        {
            return WordStr;
        }

        public string LetterStatesToString()
        {
            string letterStatesStr = "";
            foreach (KeyValuePair<string, LetterState> letter in LetterStates)
            {
               letterStatesStr += $"{letter.Key}, {letter.Value}\n";
            }
            return letterStatesStr;
        }

        public static Word CreateWord(string wordStr)
        {
            return new Word() {
                WordStr = wordStr,
                Length = wordStr.Length,
                Letters = wordStr.ToCharArray(),
                LetterStates = new Dictionary<string, LetterState>()
            };
        }
    }
}

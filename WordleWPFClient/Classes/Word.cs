using System;
using System.Collections.Generic;
using System.Text;

namespace WordleWPFClient.Classes
{
    class Word
    {
        public string wordString { get; set; }
        public int wordLength { get; set; }
        public Letter[] letters { get; set; }


        public Word(string wordString, Letter[] letters)
        {
            this.wordString = wordString;
            this.wordLength = wordString.Length;
            this.letters = letters;
        }

        public override string ToString()
        {
            return wordString;
        }
    }
}

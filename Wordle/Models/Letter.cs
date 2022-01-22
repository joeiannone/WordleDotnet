using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    class Letter
    {
        private char letterStr;
        public int wordPos;

        public Letter(char letterStr, int wordPos)
        {
            this.letterStr = letterStr;
            this.wordPos = wordPos;
        }
        public override string ToString()
        {
            return letterStr.ToString();
        }
    }
}

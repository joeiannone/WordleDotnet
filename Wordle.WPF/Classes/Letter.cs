using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WordleWPFClient.Classes
{
    class Letter
    {
        public char Character { get; set; }
        public int Row { get; set; }
        public int Position { get; set; }
        public bool isCorrect { get; set; } = false;
        public bool isInWord { get; set; } = false;
        public TextBox associatedTextBox { get; set; }

        public Letter()
        {

        }

        public override string ToString()
        {
            return $"{Character} {Row} {Position} {isCorrect.ToString()} {isInWord.ToString()}";
        }
    }
}

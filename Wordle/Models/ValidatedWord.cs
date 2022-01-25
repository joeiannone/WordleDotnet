using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    public class ValidatedWord : Word
    {
        public Boolean IsValid { get; set;  }

    }
}

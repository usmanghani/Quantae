using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public enum QuestionDimension
    {
        Unknown = 0,

        // updates vocab history.
        Vocab = 1,

        // detects major weakness.
        Understanding = 2,

        // detects learning type.
        Grammar = 3,

        NounConjugation = 4,
        VerbConjugation = 5,
    }
}

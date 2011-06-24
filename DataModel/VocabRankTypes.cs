using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public enum VocabRankTypes
    {
        Unknown = 0,
        Wrong = 1,
        SeenInSampleOrQuestion = 2,
        CorrectOrSeenInIntro = 3,
    }
}

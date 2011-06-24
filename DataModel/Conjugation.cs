using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public abstract class Conjugation
    {
        public GenderRule Gender { get; set; }
        public NumberRule Number { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class ContextualAnalysis
    {
        public List<int[]> Boxes { get; set; }

        public ContextualAnalysis()
        {
            this.Boxes = new List<int[]>();
        }
    }
}

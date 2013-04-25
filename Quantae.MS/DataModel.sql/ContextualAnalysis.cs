using System.Collections.Generic;

namespace Quantae.DataModel.Sql
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
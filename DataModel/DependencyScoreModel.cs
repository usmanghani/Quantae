using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class DependencyScoreModel : ScoreModel
    {
        public DependencyScoreModel()
        {
            this.Entries = new List<double>();
            this.Score = 0.0;
        }
    }
}

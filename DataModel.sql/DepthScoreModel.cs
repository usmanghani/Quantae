using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel.Sql
{
    public class DepthScoreModel : ScoreModel
    {
        public DepthScoreModel()
        {
            this.Score = 0.0;
            this.Entries = new List<double>();
        }
    }
}

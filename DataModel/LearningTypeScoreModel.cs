using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class LearningTypeScoreModel
    {
        public double Score { get; set; }
        public List<int> Entries { get; set; }

        public LearningTypeScoreModel()
        {
            this.Score = default(double);
            this.Entries = new List<int>();
        }
    }
}
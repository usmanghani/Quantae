using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class LearningTypeScoreModel : ScoreModel
    {
        public LearningTypeScoreModel()
        {
            this.Score = default(double);
            this.Entries = new List<double>();
        }
    }
}
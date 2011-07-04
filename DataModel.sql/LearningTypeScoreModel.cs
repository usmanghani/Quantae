using System.Collections.Generic;

namespace Quantae.DataModel.Sql
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
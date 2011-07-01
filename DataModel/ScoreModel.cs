using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    [BsonKnownTypes(typeof(LearningTypeScoreModel), typeof(MemoryScoreModel), typeof(DepthScoreModel), typeof(DependencyScoreModel))]
    public abstract class ScoreModel
    {
        public double Score { get; set; }
        public List<double> Entries { get; set; }
    }
}

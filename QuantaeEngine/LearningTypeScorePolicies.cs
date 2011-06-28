using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public enum LearningTypeScoreAnswerScores
    {
        Unknown = 0,
        Skip = 1,
        Wrong = 2,
        Right = 3
    }

    public class LearningTypeScorePolicies
    {
        private static double defaultLearningTypeScore = 1.0;
        private static int windowSize = 10;
        private static double[] weights = { 0.052476151, 0.057995116, 0.064094515, 0.070835394, 0.078285217, 0.086518546, 0.095617781, 0.10567399, 0.116787821, 0.129070503, 0.142644967 };

        public static double CalculateLearningTypeScore(UserProfile userProfile, LearningTypeScoreAnswerScores score)
        {
            if (userProfile.LearningTypeScore.Entries.Count >= windowSize)
            {
                var lastNScores = userProfile.LearningTypeScore.Entries.Skip(Math.Max(0, userProfile.LearningTypeScore.Entries.Count() - windowSize)).Take(windowSize);
                return Enumerable.Sum(Enumerable.Zip(lastNScores, weights, (s, w) => s * w));
            }

            return defaultLearningTypeScore;
        }
    }
}

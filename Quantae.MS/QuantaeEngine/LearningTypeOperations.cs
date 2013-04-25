using Quantae.DataModel;

namespace Quantae.Engine
{
    public class LearningTypeOperations
    {
        public static void UpdateLearningTypeScore(UserProfile profile, AnswerScore score)
        {
            if (score == AnswerScore.Unknown)
            {
                return;
            }

            profile.Learning.LearningTypeScore.Entries.Insert(0, (double)score);
            profile.Learning.LearningTypeScore.Score = LearningTypeScorePolicies.CalculateLearningTypeScore(profile);
        }
    }
}
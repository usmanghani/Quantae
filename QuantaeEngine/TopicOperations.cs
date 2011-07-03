using Quantae.DataModel;

namespace Quantae.Engine
{
    public class TopicOperations
    {
        public static void UpdateAnswerDimensionCounts(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            if (score == AnswerScore.Right)
            {
                profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionSuccessCount[answerDimension]++;
            }
            else if (score == AnswerScore.Wrong)
            {
                profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionFailureCount[answerDimension]++;
            }
        }
    }
}

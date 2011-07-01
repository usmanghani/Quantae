using Quantae.DataModel;

namespace Quantae.Engine
{
    public class TopicOperations
    {
        public static void UpdateAnswerDimensionCounts(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            if (score == AnswerScore.Right)
            {
                profile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionSuccessCount[answerDimension]++;
            }
            else if (score == AnswerScore.Wrong)
            {
                profile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionFailureCount[answerDimension]++;
            }
        }
    }
}

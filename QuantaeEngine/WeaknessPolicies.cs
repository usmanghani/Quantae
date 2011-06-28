using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class WeaknessPolicies
    {
        private static double genderMinPassPercentage = 0.6;
        private static double numberMinPassPercentage = 0.6;
        private static double umbrellaTopicPassPercentage = 0.6;

        public static bool IsGenderWeak(UserProfile userProfile)
        {
            var successes = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionSuccessCount.Where(a => a.Key == AnswerDimension.NounConjugationGender || a.Key == AnswerDimension.VerbConjugationGender).Sum(kvp => kvp.Value);
            var failures = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionFailureCount.Where(a => a.Key == AnswerDimension.NounConjugationGender || a.Key == AnswerDimension.VerbConjugationGender).Sum(kvp => kvp.Value);

            double score = (double)successes / (successes + failures);
            return score >= genderMinPassPercentage;
        }

        public static bool IsNumberWeak(UserProfile userProfile)
        {
            var successes = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionSuccessCount.Where(a => a.Key == AnswerDimension.NounConjugationNumber || a.Key == AnswerDimension.VerbConjugationNumber).Sum(kvp => kvp.Value);
            var failures = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.AnswerDimensionFailureCount.Where(a => a.Key == AnswerDimension.NounConjugationNumber || a.Key == AnswerDimension.VerbConjugationNumber).Sum(kvp => kvp.Value);

            double score = (double)successes / (successes + failures);
            return score >= numberMinPassPercentage;
        }


        public static bool IsUmbrellaTopicWeak(UserProfile userProfile, string umbrellaTopic)
        {
            var successes = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.UmbrellaTopicSuccessCount[umbrellaTopic];
            var failures = userProfile.CurrentState.CourseStateMachineState.CurrentTopic.UmbrellaTopicFailureCount[umbrellaTopic];

            double score = (double)successes / (successes + failures);
            return score >= umbrellaTopicPassPercentage;
        }
    }
}

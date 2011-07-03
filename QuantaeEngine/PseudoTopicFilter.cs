using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class PseudoTopicFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile profile, Sentence sentence)
        {
            TopicHistoryItem thi = profile.CurrentState.CourseLocationInfo.CurrentTopic;

            if (!thi.IsPseudoTopic)
            {
                return true;
            }

            if (thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.GenderAgreement)
            {
                return sentence.RoleConjugationPairs.Any(qt => qt.Item2.Gender != GenderRule.Unknown);
            }
            else if (thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.NumberAgreement)
            {
                return sentence.RoleConjugationPairs.Any(qt => qt.Item2.Number != NumberRule.Unknown);
            }
            else if(thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.UmbrellaTopic)
            {
                string umbrellaTopicName = thi.WeaknessForPseudoTopic.UmbrellaTopicName;
                if (sentence.Tags.Any(u => u.Equals(umbrellaTopicName, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

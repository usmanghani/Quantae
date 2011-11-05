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

            bool result = false;

            // deactivate yourself when not in pseudo topic
            if (!thi.IsPseudoTopic)
            {
                result = true;
            }
            else if (thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.GenderAgreement)
            {
                result = sentence.RoleConjugationPairs.Any(qt => qt.Item2.Gender != GenderRule.Unknown);
            }
            else if (thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.NumberAgreement)
            {
                result = sentence.RoleConjugationPairs.Any(qt => qt.Item2.Number != NumberRule.Unknown);
            }
            else if(thi.WeaknessForPseudoTopic.WeaknessType == WeaknessType.UmbrellaTopic)
            {
                string umbrellaTopicName = thi.WeaknessForPseudoTopic.UmbrellaTopicName;
                result = sentence.Tags.Any(u => u.Equals(umbrellaTopicName, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }
    }
}

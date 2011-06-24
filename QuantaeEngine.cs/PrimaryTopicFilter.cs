using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class PrimaryTopicFilter
    {
        public bool IsSentenceValid(Sentence sentence, UserProfile user)
        {
            return user.CurrentState.CurrentTopic.Equals(sentence.PrimaryTopic);
        }
    }
}

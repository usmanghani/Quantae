using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class GetNextTopicResult
    {
        public bool Success { get; private set; }
        public bool IsPseudo { get; private set; }
        public WeaknessType WeaknessType { get; private set; }
        public Topic TargetTopic { get; private set; }

        public GetNextTopicResult(bool success, bool isPseudo, WeaknessType weaknessType, Topic targetTopic)
        {
            this.Success = success;
            this.IsPseudo = isPseudo;
            this.WeaknessType = weaknessType;
            this.TargetTopic = targetTopic;
        }
    }
}

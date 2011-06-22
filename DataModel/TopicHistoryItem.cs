using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class TopicHistoryItem : HistoryItem
    {
        public Topic Topic { get; set; }
        public Dictionary<AnswerDimension, int> SuccessCount { get; set; }
        public Dictionary<AnswerDimension, int> FailureCount { get; set; }
    }
}

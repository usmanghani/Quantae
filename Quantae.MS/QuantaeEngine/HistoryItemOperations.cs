using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class HistoryItemOperations
    {
        public static void UpdateHistoryItemWithSuccessFailureAndTimestamp(HistoryItem historyItem, AnswerScore score)
        {
            historyItem.LastTimestamp = DateTime.UtcNow;

            if (score == AnswerScore.Right)
            {
                historyItem.SuccessCount++;
            }

            if (score == AnswerScore.Wrong)
            {
                historyItem.FailureCount++;
            }
        }
    }
}

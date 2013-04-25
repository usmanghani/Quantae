using System;

namespace Quantae.DataModel
{
    public abstract class HistoryItem
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public DateTime LastTimestamp { get; set; }
    }
}
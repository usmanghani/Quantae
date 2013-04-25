using System;

namespace Quantae.DataModel.Sql
{
    public abstract class HistoryItem : QuantaeObject
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public DateTime LastTimestamp { get; set; }
    }
}
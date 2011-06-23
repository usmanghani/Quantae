﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class TopicHistoryItem : HistoryItem
    {
        public Topic Topic { get; set; }

        /// <summary>
        /// These are used to calculate major and minor weaknesses on the user profile.
        /// A failure on Understanding Answer Dimension means its a major weakness.
        /// Understanding Answer Dimension is Pass if Success / (Success + Failure) >= 0.8
        /// </summary>
        public Dictionary<AnswerDimension, int> AnswerDimensionSuccessCount { get; set; }
        public Dictionary<AnswerDimension, int> AnswerDimensionFailureCount { get; set; }
    }
}

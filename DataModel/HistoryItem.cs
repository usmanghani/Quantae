﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public abstract class HistoryItem
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}

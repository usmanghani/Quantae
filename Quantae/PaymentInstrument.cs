﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public enum PaymentInstrument
    {
        Unknown = 0,
        Visa = 1, 
        MasterCard = 2,
        PayPal = 3,
        GoogleCheckout = 4,
        AmazonCheckout = 5,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class PaymentInformation
    {
        public PaymentTypes PaymentType { get; set; }
        public PaymentInstrument Instrument { get; set; }
        public PaymentInstrumentInfo InstrumentInfo { get; set; }


        public PaymentInformation()
        {
            PaymentType = PaymentTypes.Free;
            Instrument = PaymentInstrument.Unknown;
            InstrumentInfo = new NullPaymentInstrumentInfo();
        }
    }
}

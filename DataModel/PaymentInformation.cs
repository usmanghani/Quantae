namespace Quantae.DataModel
{
    public class PaymentInformation
    {
        public PaymentTypes PaymentType { get; set; }
        public PaymentInstrument Instrument { get; set; }
        public PaymentInstrumentInfo InstrumentInfo { get; set; }

        public PaymentInformation()
        {
            this.PaymentType = PaymentTypes.Free;
            this.Instrument = PaymentInstrument.Unknown;
            this.InstrumentInfo = new NullPaymentInstrumentInfo();
        }
    }
}
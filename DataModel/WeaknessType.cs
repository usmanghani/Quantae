using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public enum WeaknessType
    {
        Unknown = 0,

        // Minor weakness
        NumberAgreement = 1,
        GenderAgreement = 2,

        // Major weakness.
        Understanding = 3,

        UmbrellaTopic = 4,
    }
}

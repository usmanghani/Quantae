using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class ExtrasHub : Hub
    {
        public override List<HubAction> Actions { get; set; }
        public ExtrasHub()
        {
            this.Actions = new List<HubAction>();
        }
    }
}

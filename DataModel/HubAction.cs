using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    public class HubAction
    {
        public string Choice { get; set; }

        public override string ToString()
        {
            return this.Choice;
        }
    }
}

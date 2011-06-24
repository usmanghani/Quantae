using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class QuantaeObjectId<T>
    {
        public QuantaeObjectId(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }

        public static explicit operator T(QuantaeObjectId<T> obj)
        {
            return obj.Value;
        }

        public static implicit operator QuantaeObjectId<T>(T val)
        {
            return new QuantaeObjectId<T>(val);
        }
    }
}

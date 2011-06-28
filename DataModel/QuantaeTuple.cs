using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class QuantaeTuple<T1, T2> : IEquatable<QuantaeTuple<T1, T2>>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public QuantaeTuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public bool Equals(QuantaeTuple<T1, T2> other)
        {
            return this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2);
        }

        public override bool Equals(object obj)
        {
            QuantaeTuple<T1, T2> other = obj as QuantaeTuple<T1, T2>;
            return this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode();
        }
    }

    public class QuantaeTuple<T1, T2, T3> : IEquatable<QuantaeTuple<T1, T2, T3>>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }

        public QuantaeTuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public bool Equals(QuantaeTuple<T1, T2, T3> other)
        {
            return this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2) && this.Item3.Equals(other.Item3);
        }

        public override bool Equals(object obj)
        {
            QuantaeTuple<T1, T2, T3> other = obj as QuantaeTuple<T1, T2, T3>;
            return this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2) && this.Item3.Equals(other.Item3);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode() + this.Item3.GetHashCode();
        }

    }
}

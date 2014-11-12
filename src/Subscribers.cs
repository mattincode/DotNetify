using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public struct Subscribers : ICloneable, IEquatable<Subscribers>
    {
        public uint Count { get; private set; }

        public IntPtr SubscriberNames { get; private set; }

        public Subscribers(uint count, IntPtr subscribers)
            : this()
        {
            this.Count = count;
            this.SubscriberNames = subscribers;
        }

        public object Clone()
        {
            return new Subscribers(this.Count, this.SubscriberNames);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is Subscribers) ? this.Equals((Subscribers)obj) : false;
        }

        public bool Equals(Subscribers other)
        {
            return (this.Count == other.Count) && (this.SubscriberNames == other.SubscriberNames);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.Count, this.SubscriberNames);
        }

        public static bool operator ==(Subscribers left, Subscribers right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Subscribers left, Subscribers right)
        {
            return !(left == right);
        }
    }
}

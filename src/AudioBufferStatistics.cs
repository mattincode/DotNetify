using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public struct AudioBufferStatistics : ICloneable, IEquatable<AudioBufferStatistics>
    {
        public int Samples { get; private set; }

        public int Stutter { get; private set; }

        public AudioBufferStatistics(int samples, int stutter)
            : this()
        {
            this.Samples = samples;
            this.Stutter = stutter;
        }

        public object Clone()
        {
            return new AudioBufferStatistics(this.Samples, this.Stutter);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is AudioBufferStatistics) ? this.Equals((AudioBufferStatistics)obj) : false;
        }

        public bool Equals(AudioBufferStatistics other)
        {
            return (this.Samples == other.Samples) && (this.Stutter == other.Stutter);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.Samples, this.Stutter);
        }

        public static bool operator ==(AudioBufferStatistics left, AudioBufferStatistics right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AudioBufferStatistics left, AudioBufferStatistics right)
        {
            return !(left == right);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Describes the audio served by spotify.
    /// </summary>
    public struct AudioFormat : ICloneable, IEquatable<AudioFormat>
    {
        /// <summary>
        /// Sample type.
        /// </summary>
        public SampleType SampleType { get; private set; }

        /// <summary>
        /// Audio sample rate, in samples per second.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Number of channels. Currently 1 or 2.
        /// </summary>
        public int Channels { get; private set; }

        public AudioFormat(SampleType sampleType, int sampleRate, int channels)
            : this()
        {
            this.SampleType = sampleType;
            this.SampleRate = sampleRate;
            this.Channels = channels;
        }

        public object Clone()
        {
            return new AudioFormat(this.SampleType, this.SampleRate, this.Channels);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is AudioFormat) ? this.Equals((AudioFormat)obj) : false;
        }

        public bool Equals(AudioFormat other)
        {
            return (this.SampleType == other.SampleType) && (this.SampleRate == other.SampleRate) &&
                   (this.Channels == other.Channels);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.SampleType, this.SampleRate, this.Channels);
        }

        public static bool operator ==(AudioFormat left, AudioFormat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AudioFormat left, AudioFormat right)
        {
            return !(left == right);
        }
    }
}

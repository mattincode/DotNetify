using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public struct MusicPackage : ICloneable, IEquatable<MusicPackage>
    {
        /// <summary>
        /// The audio format descriptor.
        /// </summary>
        public AudioFormat Format { get; private set; }

        /// <summary>
        /// The raw PCM data as described by <paramref name="format"/>.
        /// </summary>
        public IntPtr Frames { get; private set; }

        /// <summary>
        /// The frame count. If this is 0, a discontinuity has occurred (such as after a seek). 
        /// The application should flush its audio fifos, etc.
        /// </summary>
        public int FrameCount { get; private set; }

        public MusicPackage(AudioFormat format, IntPtr frames, int frameCount)
            : this()
        {
            Contract.Requires<ArgumentException>(frames != IntPtr.Zero);
            Contract.Requires<ArgumentOutOfRangeException>(frameCount >= 0);

            this.Format = format;
            this.Frames = frames;
            this.FrameCount = frameCount;
        }

        public object Clone()
        {
            return new MusicPackage(this.Format, this.Frames, this.FrameCount);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is MusicPackage) ? this.Equals((MusicPackage)obj) : false;
        }

        public bool Equals(MusicPackage other)
        {
            return (this.Format == other.Format) && (this.Frames == other.Frames) &&
                   (this.FrameCount == other.FrameCount);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.Format, this.Frames, this.FrameCount);
        }

        public static bool operator ==(MusicPackage left, MusicPackage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MusicPackage left, MusicPackage right)
        {
            return !(left == right);
        }
    }
}

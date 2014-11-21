using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a set of PCM frames.
    /// </summary>
    public struct MusicPacket : ICloneable, IEquatable<MusicPacket>
    {
        /// <summary>
        /// The audio format descriptor.
        /// </summary>
        public AudioFormat Format { get; private set; }

        /// <summary>
        /// The raw PCM data as described by <see cref="P:Format"/>.
        /// </summary>
        public IntPtr Frames { get; private set; }

        /// <summary>
        /// The frame count. If this is 0, a discontinuity has occurred (such as after a seek). 
        /// The application should flush its audio fifos, etc.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// Gets the total size in bytes of the music packet.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Format.SampleType.GetFrameSize() * this.Format.Channels * this.FrameCount;
            }
        }
        
        /// <summary>
        /// Initializes a new <see cref="MusicPacket"/>.
        /// </summary>
        /// <param name="format">The <see cref="AudioFormat"/> of the frames in <see cref="P:Frames"/>.</param>
        /// <param name="frames">A pointer to the PCM data.</param>
        /// <param name="frameCount">The amount of frames.</param>
        public MusicPacket(AudioFormat format, IntPtr frames, int frameCount)
            : this()
        {
            Contract.Requires<ArgumentException>(frames != IntPtr.Zero);
            Contract.Requires<ArgumentOutOfRangeException>(frameCount >= 0);

            this.Format = format;
            this.Frames = frames;
            this.FrameCount = frameCount;
        }

        /// <summary>
        /// Creates a flat copy of the <see cref="MusicPacket"/>.
        /// </summary>
        /// <returns>The cloned objet.</returns>
        public object Clone()
        {
            return new MusicPacket(this.Format, this.Frames, this.FrameCount);
        }

        /// <summary>
        /// Copies the frame data into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The array to copy the data into.</param>
        public void CopyTo(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentException>(buffer.Length == this.Size);

            Marshal.Copy(this.Frames, buffer, 0, this.Size);
        }

        /// <summary>
        /// Copies the frame data into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The array to copy the data into.</param>
        public void CopyTo(short[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentException>(buffer.Length == (this.Size / sizeof(short)));

            if (this.Size % sizeof(short) != 0)
            {
                throw new InvalidOperationException("The samples cannot be converted into Int16s since the amount cannot be divided without remainder.");
            }
            Marshal.Copy(this.Frames, buffer, 0, this.Size / sizeof(short));
        }

        /// <summary>
        /// Copies the frame data into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The array to copy the data into.</param>
        public void CopyTo(int[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentException>(buffer.Length == (this.Size / sizeof(int)));

            if (this.Size % sizeof(int) != 0)
            {
                throw new InvalidOperationException("The samples cannot be converted into Int32s since the amount cannot be divided without remainder.");
            }
            Marshal.Copy(this.Frames, buffer, 0, this.Size / sizeof(int));
        }

        /// <summary>
        /// Checks whether the music packet is equal to the other specified object.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns><c>true</c> if the music packet and the object are equal, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is MusicPacket) ? this.Equals((MusicPacket)obj) : false;
        }

        /// <summary>
        /// Checks whether two music packets are equal.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns><c>true</c> if the music packet and the object are equal, otherwise <c>false</c>.</returns>
        public bool Equals(MusicPacket other)
        {
            return (this.Format == other.Format) && (this.Frames == other.Frames) &&
                   (this.FrameCount == other.FrameCount);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.Format, this.Frames, this.FrameCount);
        }

        /// <summary>
        /// Converts the <see cref="P:Frames"/> into a managed array.
        /// </summary>
        /// <remarks>This will copy the frames into managed memory.</remarks>
        /// <returns>The frame data.</returns>
        public byte[] ToArray()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == this.Size);

            int size = this.Size;
            byte[] data = new byte[size];
            Marshal.Copy(this.Frames, data, 0, size);
            return data;
        }

        /// <summary>
        /// Converts the <see cref="P:Frames"/> into a <see cref="MemoryStream"/>.
        /// </summary>
        /// <remarks>This will copy the frames into managed memory.</remarks>
        /// <returns>
        /// The <see cref="P:Frames"/> as <see cref="MemoryStream"/>. It is your responsibility to dispose the <see cref="MemoryStream"/>
        /// after you're done using it.
        /// </returns>
        public MemoryStream ToStream()
        {
            Contract.Ensures(Contract.Result<MemoryStream>() != null);

            return new MemoryStream(this.ToArray(), false);
        }

        /// <summary>
        /// Checks whether two <see cref="MusicPacket"/>s are equal.
        /// </summary>
        /// <param name="left">The first operand.</param>
        /// <param name="right">The second operand.</param>
        /// <returns><c>true</c> if the music packets are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(MusicPacket left, MusicPacket right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks whether two <see cref="MusicPacket"/>s are not equal.
        /// </summary>
        /// <param name="left">The first operand.</param>
        /// <param name="right">The second operand.</param>
        /// <returns><c>true</c> if the music packets are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(MusicPacket left, MusicPacket right)
        {
            return !(left == right);
        }
    }
}

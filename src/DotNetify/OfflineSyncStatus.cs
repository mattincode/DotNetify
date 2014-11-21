using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Offline sync status.
    /// </summary>
    public struct OfflineSyncStatus : ICloneable, IEquatable<OfflineSyncStatus>
    {
        /// <summary>
        /// The bytes that have already been on the device, so they didn't have to be synchronized.
        /// </summary>
        public ulong AlreadySynchronizedBytes { get; private set; }

        /// <summary>
        /// The tracks that have already been on the device, so they didn't have to be synchronized.
        /// </summary>
        public int AlreadySynchronizedTracks { get; private set; }

        /// <summary>
        /// Tracks where something went wrong while synchronizing.
        /// </summary>
        public int ErroneousTracks { get; private set; }

        /// <summary>
        /// Backing field.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        private bool _IsSyncing;

        /// <summary>
        /// Indicates whether the sync operation is in progress.
        /// </summary>
        public bool IsSyncing
        {
            get
            {
                return _IsSyncing;
            }
            private set
            {
                _IsSyncing = value;
            }
        }

        /// <summary>
        /// The remaining bytes to synchronize.
        /// </summary>
        public ulong QueuedBytes { get; private set; }

        /// <summary>
        /// The remaining tracks to synchronize.
        /// </summary>
        public int QueuedTracks { get; private set; }

        /// <summary>
        /// The already synchronized tracks.
        /// </summary>
        public ulong SynchronizedBytes { get; private set; }

        /// <summary>
        /// The already synchronized tracks.
        /// </summary>
        public int SynchronizedTracks { get; private set; }

        /// <summary>
        /// The tracks that will not be copied for whatever reason.
        /// </summary>
        public int WillNotCopyTracks { get; private set; }

        public OfflineSyncStatus(
                            int queuedTracks,
                            ulong queuedBytes,
                            int finishedTracks,
                            ulong finishedBytes,
                            int copiedTracks,
                            ulong copiedBytes,
                            int willNotCopyTracks,
                            int erroneousTracks,
                            bool isSyncing
                        )
            : this()
        {
            this.QueuedTracks = queuedTracks;
            this.QueuedBytes = queuedBytes;
            this.AlreadySynchronizedTracks = finishedTracks;
            this.AlreadySynchronizedBytes = finishedBytes;
            this.SynchronizedTracks = copiedTracks;
            this.SynchronizedBytes = copiedBytes;
            this.WillNotCopyTracks = willNotCopyTracks;
            this.ErroneousTracks = erroneousTracks;
            this.IsSyncing = isSyncing;
        }

        public object Clone()
        {
            return new OfflineSyncStatus(
                this.QueuedTracks, this.QueuedBytes, 
                this.AlreadySynchronizedTracks, this.AlreadySynchronizedBytes, 
                this.SynchronizedTracks, this.SynchronizedBytes,
                this.WillNotCopyTracks, this.ErroneousTracks,
                this.IsSyncing
            );
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is OfflineSyncStatus) ? this.Equals((OfflineSyncStatus)obj) : false;
        }

        public bool Equals(OfflineSyncStatus other)
        {
            return (this.QueuedTracks == other.QueuedTracks) && (this.QueuedBytes == other.QueuedBytes) &&
                   (this.AlreadySynchronizedTracks == other.AlreadySynchronizedTracks) && (this.AlreadySynchronizedBytes == other.AlreadySynchronizedBytes) &&
                   (this.SynchronizedTracks == other.SynchronizedTracks) && (this.SynchronizedBytes == other.SynchronizedBytes) &&
                   (this.WillNotCopyTracks == other.WillNotCopyTracks) && (this.ErroneousTracks == other.ErroneousTracks) &&
                   (this.IsSyncing == other.IsSyncing);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(
                HashF.GetHashCode(
                    this.QueuedTracks, this.QueuedBytes, 
                    this.AlreadySynchronizedTracks, this.AlreadySynchronizedBytes, 
                    this.SynchronizedTracks, this.SynchronizedBytes
                ),
                this.WillNotCopyTracks,
                this.ErroneousTracks,
                this.IsSyncing
            );
        }

        public static bool operator !=(OfflineSyncStatus left, OfflineSyncStatus right)
        {
            return !(left == right);
        }

        public static bool operator ==(OfflineSyncStatus left, OfflineSyncStatus right)
        {
            return left.Equals(right);
        }
    }
}

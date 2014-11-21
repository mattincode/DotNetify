using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class OfflineSyncStatusChangedEventArgs : SpotifyEventArgs
    {
        public OfflineSyncStatus SyncStatus { get; private set; }

        public OfflineSyncStatusChangedEventArgs(Session session, OfflineSyncStatus syncStatus)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.SyncStatus = syncStatus;
        }
    }
}

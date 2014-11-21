using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class MusicDeliveryEventArgs : SpotifyEventArgs
    {
        public MusicPacket MusicData { get; private set; }

        public MusicDeliveryEventArgs(Session session, MusicPacket musicData)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.MusicData = musicData;
        }
    }
}

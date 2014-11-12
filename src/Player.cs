using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Player : ObservableObject
    {
        private bool _IsPlaying;

        public bool IsPlaying
        {
            get
            {
                return _IsPlaying;
            }
            private set
            {
                this.SetProperty(ref _IsPlaying, value);
            }
        }

        private Session _Session;

        public Session Session
        {
            get
            {
                return _Session;
            }
            private set
            {
                this.SetProperty(ref _Session, value);
            }
        }

        public Player(Session s)
        {
            Contract.Requires<ArgumentNullException>(s != null);

            this.Session = s;
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public struct ProxyConfig : ICloneable, IEquatable<ProxyConfig>
    {
        /// <summary>
        /// Url to the proxy server that should be used.
        /// The format is protocol://<host>:port (where protocal is http/https/socks4/socks5)
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Username to authenticate with the proxy server.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Password to authenticate with the proxy server.
        /// </summary>
        public string Password { get; private set; }

        public ProxyConfig(string url, string username, string password)
            : this()
        {
            this.Url = url;
            this.Username = username;
            this.Password = password;
        }

        public object Clone()
        {
            return new ProxyConfig(this.Url, this.Username, this.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is ProxyConfig) ? this.Equals((ProxyConfig)obj) : false;
        }

        public bool Equals(ProxyConfig other)
        {
            return (this.Url == other.Url) && (this.Username == other.Username) &&
                   (this.Password == other.Password);
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.Url, this.Username, this.Password);
        }

        public static bool operator ==(ProxyConfig left, ProxyConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProxyConfig left, ProxyConfig right)
        {
            return !(left == right);
        }
    }
}

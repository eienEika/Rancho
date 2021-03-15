using System;
using NetCoreServer;

namespace Rancho.Server
{
    internal sealed class Session : TcpSession
    {
        public Session(TcpServer server) : base(server)
        {
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Server.Multicast(buffer, offset, size);
        }
    }
}
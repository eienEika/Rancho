using System;
using System.Collections.Generic;
using NetCoreServer;
using Rancho.Protocol;

namespace Rancho.Server
{
    internal sealed class Server : TcpServer
    {
        public Server(string address, ushort port) : base(address, port)
        {
            OptionKeepAlive = true;
            OptionDualMode = true;
        }

        public Dictionary<Guid, User> Users { get; } = new();

        public void Multicast(Message message)
        {
            foreach (var user in Users.Values)
            {
                user.SendAsync(message);
            }
        }

        protected override TcpSession CreateSession()
        {
            return new Session(this);
        }
    }
}
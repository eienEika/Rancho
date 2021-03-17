using NetCoreServer;
using Rancho.Protocol;

namespace Rancho.Server
{
    internal sealed class Session : TcpSession
    {
        public Session(TcpServer server) : base(server)
        {
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            foreach (var message in Message.Read(buffer, (int) offset, (int) size))
            {
                Request.Process(Program.Server.Users.ContainsKey(Id) ? Program.Server.Users[Id] : new User(this),
                    message);
            }
        }
    }
}
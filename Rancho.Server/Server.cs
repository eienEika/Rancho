using NetCoreServer;

namespace Rancho.Server
{
    internal sealed class Server : TcpServer
    {
        public Server(string address, ushort port) : base(address, port)
        {
            OptionKeepAlive = true;
            OptionDualMode = true;
        }

        protected override TcpSession CreateSession()
        {
            return new Session(this);
        }
    }
}
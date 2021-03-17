using System;
using NetCoreServer;
using Rancho.Protocol;

namespace Rancho.Client
{
    internal sealed class Client : TcpClient
    {
        public Client(string remoteAddress, ushort remotePort) : base(remoteAddress, remotePort)
        {
            OptionDualMode = true;
            OptionKeepAlive = true;
        }

        public event EventHandler OnClientConnected;
        public event EventHandler<MessageEventArgs> OnMessage;

        protected override void OnConnected()
        {
            var raiseClientConnectedEvent = OnClientConnected;
            raiseClientConnectedEvent?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            foreach (var message in Message.Read(buffer, (int) offset, (int) size))
            {
                var raiseOnMessage = OnMessage;
                raiseOnMessage?.Invoke(this, new MessageEventArgs(message));
            }
        }

        public void SendAsync(Message message)
        {
            SendAsync(message.Write());
        }
    }
}
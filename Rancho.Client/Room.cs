using Qml.Net;
using Rancho.Protocol.Messages;

namespace Rancho.Client
{
    [Signal(QmlHelper.UserConnectedSignal, NetVariantType.String)]
    [Signal(QmlHelper.ChatMessageSignal, NetVariantType.String, NetVariantType.String)]
    [Signal(QmlHelper.UrlChangedSignal, NetVariantType.String)]
    [Signal(QmlHelper.PauseChangedSignal, NetVariantType.Bool)]
    public sealed class Room
    {
        private Client _client;

        public void Connect(string ip, int port, string username)
        {
            if (_client != null)
            {
                return;
            }

            _client = new Client(ip, (ushort) port);

            _client.OnClientConnected += (_, _) => _client.SendAsync(HelloMsg.Create(username));
            _client.OnMessage += MessageHandler;

            _client.ConnectAsync();
        }

        private void MessageHandler(object sender, MessageEventArgs args)
        {
            QmlHelper.ActivateSignal(this, args.Message.MessageType, args.Message.Data);
        }

        public void Disconnect()
        {
            _client?.Disconnect();
        }

        public void SendChatMessage(string text)
        {
            _client.SendAsync(ChatMessageMsgClient.Create(text));
        }

        public void SetUrl(string url)
        {
            _client.SendAsync(SetUrlMsgClient.Create(url));
        }

        public void SetPause(bool pause)
        {
            _client.SendAsync(SetPauseMsgClient.Create(pause));
        }
    }
}
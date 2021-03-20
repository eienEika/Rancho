using Rancho.Protocol;
using Rancho.Protocol.Messages;

namespace Rancho.Server.Requests
{
    [Request(MessageType.ChatMessageClient)]
    internal sealed class ChatMessageRequest : Request
    {
        public ChatMessageRequest(User user, Message requestMessage) : base(user, requestMessage)
        {
        }

        protected override void ProcessImpl()
        {
            MulticastMessage = ChatMessageMsgServer.Create(User.Username, RequestMessage.Data[0]);
        }
    }
}
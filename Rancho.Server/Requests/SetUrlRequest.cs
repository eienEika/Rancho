using Rancho.Protocol;
using Rancho.Protocol.Messages;

namespace Rancho.Server.Requests
{
    [Request(MessageType.SetUrlClient)]
    internal sealed class SetUrlRequest : Request
    {
        public SetUrlRequest(User user, Message requestMessage) : base(user, requestMessage)
        {
        }

        protected override void ProcessImpl()
        {
            MulticastMessage = SetUrlMsgServer.Create(RequestMessage.Data[0]);
        }
    }
}
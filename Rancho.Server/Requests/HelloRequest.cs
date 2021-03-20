using Rancho.Protocol;
using Rancho.Protocol.Messages;

namespace Rancho.Server.Requests
{
    [Request(MessageType.Hello)]
    internal sealed class HelloRequest : Request
    {
        public HelloRequest(User user, Message requestMessage) : base(user, requestMessage)
        {
        }

        protected override void ProcessImpl()
        {
            User.Authenticate(RequestMessage.Data[0]);

            MulticastMessage = UserConnectedMsg.Create(User.Username);
        }
    }
}
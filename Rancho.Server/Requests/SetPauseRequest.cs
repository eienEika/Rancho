using Rancho.Protocol;
using Rancho.Protocol.Messages;

namespace Rancho.Server.Requests
{
    internal sealed class SetPauseRequest : Request
    {
        public SetPauseRequest(User user, Message requestMessage) : base(user, requestMessage)
        {
        }

        protected override void ProcessImpl()
        {
            MulticastMessage = SetPauseMsgServer.Create(RequestMessage.Data[0]);
        }
    }
}
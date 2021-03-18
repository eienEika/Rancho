using Rancho.Protocol;
using Rancho.Server.Requests;

namespace Rancho.Server
{
    internal abstract class Request
    {
        protected Request(User user, Message requestMessage)
        {
            User = user;
            RequestMessage = requestMessage;
        }

        protected User User { get; }
        protected Message RequestMessage { get; }
        protected Message ResponseMessage { get; set; }
        protected Message MulticastMessage { get; set; }

        protected abstract void ProcessImpl();

        public static void Process(User user, Message message)
        {
            if (!user.IsAuth && message.MessageType != MessageType.Hello)
            {
                return;
            }

            Request request = message.MessageType switch
            {
                MessageType.Hello => new HelloRequest(user, message),
                MessageType.ChatMessageClient => new ChatMessageRequest(user, message),
                MessageType.SetPauseClient => new SetPauseRequest(user, message),
                MessageType.SetUrlClient => new SetUrlRequest(user, message),
                _ => null,
            };

            if (request == null)
            {
                return;
            }

            request.ProcessImpl();

            request.Response();
            request.Multicast();
        }

        private void Multicast()
        {
            if (MulticastMessage != null)
            {
                Program.Server.Multicast(MulticastMessage);
            }
        }

        private void Response()
        {
            if (ResponseMessage != null)
            {
                User.SendAsync(ResponseMessage);
            }
        }
    }
}
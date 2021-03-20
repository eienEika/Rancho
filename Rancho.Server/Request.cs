using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rancho.Protocol;

namespace Rancho.Server
{
    internal abstract class Request
    {
        private static readonly Dictionary<MessageType, ConstructorInfo> Requests = new();

        static Request()
        {
            foreach (var type in Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "Rancho.Server.Requests" && t.IsSubclassOf(typeof(Request))))
            {
                var attr = type.GetCustomAttribute<RequestAttribute>();
                var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null,
                    CallingConventions.HasThis, new[] {typeof(User), typeof(Message)}, null);
                if (constructor != null)
                {
                    Requests.Add(attr!.MessageType, constructor);
                }
            }
        }

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

            if (!Requests.TryGetValue(message.MessageType, out var constructor))
            {
                return;
            }

            var request = (Request) constructor.Invoke(new object[] {user, message});

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
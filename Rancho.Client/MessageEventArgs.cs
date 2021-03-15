using System;
using Rancho.Protocol;

namespace Rancho.Client
{
    internal sealed class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; }
    }
}
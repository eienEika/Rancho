using System;

namespace Rancho.Protocol
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class MessageAttribute : Attribute
    {
        public MessageAttribute(MessageType messageType)
        {
            MessageType = messageType;
        }

        public MessageType MessageType { get; }
    }
}
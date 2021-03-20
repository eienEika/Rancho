using System;
using Rancho.Protocol;

namespace Rancho.Server
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class RequestAttribute : Attribute
    {
        public RequestAttribute(MessageType messageType)
        {
            MessageType = messageType;
        }

        public MessageType MessageType { get; }
    }
}
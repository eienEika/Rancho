using System.Collections.Generic;
using Qml.Net;
using Rancho.Protocol;

namespace Rancho.Client
{
    internal static class QmlHelper
    {
        public const string UserConnectedSignal = "userConnectedSignal";
        public const string ChatMessageSignal = "chatMessageSignal";
        public const string UrlChangedSignal = "urlChangedSignal";
        public const string PauseChangedSignal = "pauseChangedSignal";

        private static readonly Dictionary<MessageType, string> SignalnameLookup = new()
        {
            [MessageType.UserConnected] = UserConnectedSignal,
            [MessageType.ChatMessageServer] = ChatMessageSignal,
            [MessageType.SetUrlServer] = UrlChangedSignal,
            [MessageType.SetPauseServer] = PauseChangedSignal,
        };

        public static void ActivateSignal(object instance, MessageType messageType, dynamic[] data)
        {
            Program.Application.Dispatch(() => instance.ActivateSignal(SignalnameLookup[messageType], data));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using Rancho.Protocol.Messages;

namespace Rancho.Protocol
{
    public abstract class Message
    {
        public abstract MessageType MessageType { get; }
        public abstract dynamic[] Data { get; }
        protected CborWriter Writer { get; } = new(CborConformanceMode.Canonical, true, true);

        public static IEnumerable<Message> Read(ReadOnlyMemory<byte> data)
        {
            var reader = new CborReader(data, CborConformanceMode.Canonical, true);

            while (reader.BytesRemaining > 0)
            {
                try
                {
                    if (reader.PeekState() != CborReaderState.UnsignedInteger)
                    {
                        yield break;
                    }
                }
                catch (CborContentException)
                {
                    yield break;
                }

                var messageTypeULong = reader.ReadUInt64();
                if (!Enum.IsDefined(typeof(MessageType), messageTypeULong))
                {
                    yield break;
                }

                var messageType = (MessageType) messageTypeULong;

                Message message = messageType switch
                {
                    MessageType.Connect => new UserConnectedMsg(),
                    MessageType.ChatMessage => new ChatMessageMsg(),
                    MessageType.SetUrl => new SetUrlMsg(),
                    MessageType.SetPause => new SetPauseMsg(),
                    _ => null,
                };

                if (message == null || !message.Unpack(reader))
                {
                    yield break;
                }

                yield return message;
            }
        }

        protected abstract void PackImpl();
        protected abstract bool Unpack(CborReader reader);

        public byte[] Pack()
        {
            Writer.WriteUInt64((ulong) MessageType);
            PackImpl();

            return Writer.Encode();
        }
    }
}
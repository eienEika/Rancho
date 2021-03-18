using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using System.IO;
using Rancho.Protocol.Messages;

namespace Rancho.Protocol
{
    public abstract class Message
    {
        public abstract MessageType MessageType { get; }
        public abstract dynamic[] Data { get; }

        private protected CborWriter Writer { get; } = new(CborConformanceMode.Canonical, true, true);

        private protected abstract bool ReadData(CborReader reader);
        private protected abstract void WriteData();

        public static IEnumerable<Message> Read(byte[] data, int offset, int size)
        {
            using var stream = new MemoryStream(data, offset, size);

            while (stream.Length - stream.Position > 0)
            {
                if (stream.Length - stream.Position < 2)
                {
                    yield break;
                }

                var lengthBuffer = new byte[2];
                stream.Read(lengthBuffer);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthBuffer);
                }

                var length = BitConverter.ToUInt16(lengthBuffer);

                if (stream.Length - stream.Position < length)
                {
                    yield break;
                }

                var messageBuffer = new byte[length];
                stream.Read(messageBuffer);

                Message message;
                try
                {
                    message = ReadCbor(messageBuffer);
                }
                catch (CborContentException)
                {
                    yield break;
                }

                if (message == null)
                {
                    yield break;
                }

                yield return message;
            }
        }

        public byte[] Write()
        {
            Writer.WriteUInt64((ulong) MessageType);
            Writer.WriteStartArray(Data.Length);
            WriteData();
            Writer.WriteEndArray();

            var buffer = new byte[2 + Writer.BytesWritten];
            Buffer.BlockCopy(BitConverter.GetBytes((ushort) Writer.BytesWritten), 0, buffer, 0, 2);
            Buffer.BlockCopy(Writer.Encode(), 0, buffer, 2, Writer.BytesWritten);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer, 0, 2);
            }

            return buffer;
        }

        private static Message ReadCbor(byte[] data)
        {
            var reader = new CborReader(data, CborConformanceMode.Canonical, true);
            MessageType messageType;
            try
            {
                if (reader.PeekState() == CborReaderState.UnsignedInteger)
                {
                    return null;
                }

                messageType = (MessageType) reader.ReadUInt64();
            }
            catch (OverflowException)
            {
                return null;
            }

            Message message = messageType switch
            {
                MessageType.Hello => new HelloMsg(),
                MessageType.ChatMessageClient => new ChatMessageMsgClient(),
                MessageType.SetUrlClient => new SetUrlMsgClient(),
                MessageType.SetPauseClient => new SetPauseMsgClient(),
                MessageType.UserConnected => new UserConnectedMsg(),
                MessageType.ChatMessageServer => new ChatMessageMsgServer(),
                MessageType.SetUrlServer => new SetUrlMsgServer(),
                MessageType.SetPauseServer => new SetPauseMsgServer(),
                _ => null,
            };

            if (message == null)
            {
                return null;
            }

            if (reader.PeekState() != CborReaderState.StartArray)
            {
                return null;
            }

            reader.ReadStartArray();

            if (!message.ReadData(reader))
            {
                return null;
            }

            if (reader.PeekState() != CborReaderState.EndArray)
            {
                return null;
            }

            reader.ReadEndArray();

            return message;
        }
    }
}
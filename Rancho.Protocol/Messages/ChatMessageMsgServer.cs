using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    [Message(MessageType.ChatMessageServer)]
    public sealed class ChatMessageMsgServer : Message
    {
        public override MessageType MessageType { get; } = MessageType.ChatMessageServer;
        public override dynamic[] Data { get; } = new dynamic[2];

        private protected override bool ReadData(CborReader reader)
        {
            if (reader.PeekState() != CborReaderState.TextString)
            {
                return false;
            }

            Data[0] = reader.ReadTextString();

            if (reader.PeekState() != CborReaderState.TextString)
            {
                return false;
            }

            Data[1] = reader.ReadTextString();

            return true;
        }

        private protected override void WriteData()
        {
            Writer.WriteTextString(Data[0]);
            Writer.WriteTextString(Data[1]);
        }

        public static ChatMessageMsgServer Create(string username, string text)
        {
            return new() {Data = {[0] = username, [1] = text}};
        }
    }
}
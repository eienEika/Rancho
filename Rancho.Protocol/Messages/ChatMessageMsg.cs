using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class ChatMessageMsg : Message
    {
        public override MessageType MessageType { get; } = MessageType.ChatMessage;
        public override dynamic[] Data { get; } = new dynamic[2];

        protected override void PackImpl()
        {
            Writer.WriteTextString(Data[0]);
            Writer.WriteTextString(Data[1]);
        }

        protected override bool Unpack(CborReader reader)
        {
            try
            {
                if (reader.PeekState() != CborReaderState.TextString)
                {
                    return false;
                }
            }
            catch (CborContentException)
            {
                return false;
            }

            Data[0] = reader.ReadTextString();

            try
            {
                if (reader.PeekState() != CborReaderState.TextString)
                {
                    return false;
                }
            }
            catch (CborContentException)
            {
                return false;
            }

            Data[1] = reader.ReadTextString();

            return true;
        }

        public static ChatMessageMsg Create(string username, string text)
        {
            return new() {Data = {[0] = username, [1] = text}};
        }
    }
}
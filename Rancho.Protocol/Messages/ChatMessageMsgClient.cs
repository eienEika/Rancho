using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class ChatMessageMsgClient : Message
    {
        public override MessageType MessageType { get; } = MessageType.ChatMessageClient;
        public override dynamic[] Data { get; } = new dynamic[1];

        private protected override void ReadData(CborReader reader)
        {
            Data[0] = reader.ReadTextString();
        }

        private protected override void WriteData()
        {
            Writer.WriteTextString(Data[0]);
        }

        public static ChatMessageMsgClient Create(string text)
        {
            return new() {Data = {[0] = text}};
        }
    }
}
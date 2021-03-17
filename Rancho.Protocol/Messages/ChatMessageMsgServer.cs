using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class ChatMessageMsgServer : Message
    {
        public override MessageType MessageType { get; } = MessageType.ChatMessageServer;
        public override dynamic[] Data { get; } = new dynamic[2];

        private protected override void ReadData(CborReader reader)
        {
            Data[0] = reader.ReadTextString();
            Data[1] = reader.ReadTextString();
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
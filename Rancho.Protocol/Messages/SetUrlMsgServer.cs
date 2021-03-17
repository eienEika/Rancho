using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class SetUrlMsgServer : Message
    {
        public override MessageType MessageType { get; } = MessageType.SetUrlServer;
        public override dynamic[] Data { get; } = new dynamic[1];

        private protected override void ReadData(CborReader reader)
        {
            Data[0] = reader.ReadTextString();
        }

        private protected override void WriteData()
        {
            Writer.WriteTextString(Data[0]);
        }

        public static SetUrlMsgServer Create(string url)
        {
            return new() {Data = {[0] = url}};
        }
    }
}
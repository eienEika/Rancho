using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class HelloMsg : Message
    {
        public override MessageType MessageType { get; } = MessageType.Hello;
        public override dynamic[] Data { get; } = new dynamic[1];

        private protected override void ReadData(CborReader reader)
        {
            Data[0] = reader.ReadTextString();
        }

        private protected override void WriteData()
        {
            Writer.WriteTextString(Data[0]);
        }

        public static HelloMsg Create(string username)
        {
            return new() {Data = {[0] = username}};
        }
    }
}
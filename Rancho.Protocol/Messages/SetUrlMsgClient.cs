using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class SetUrlMsgClient : Message
    {
        public override MessageType MessageType { get; } = MessageType.SetUrlClient;
        public override dynamic[] Data { get; } = new dynamic[1];

        private protected override bool ReadData(CborReader reader)
        {
            if (reader.PeekState() != CborReaderState.TextString)
            {
                return false;
            }

            Data[0] = reader.ReadTextString();

            return true;
        }

        private protected override void WriteData()
        {
            Writer.WriteTextString(Data[0]);
        }

        public static SetUrlMsgClient Create(string url)
        {
            return new() {Data = {[0] = url}};
        }
    }
}
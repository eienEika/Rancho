using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class UserConnectedMsg : Message
    {
        public override MessageType MessageType { get; } = MessageType.Connect;
        public override dynamic[] Data { get; } = new dynamic[1];

        protected override void PackImpl()
        {
            Writer.WriteTextString(Data[0]);
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

            return true;
        }

        public static UserConnectedMsg Create(string username)
        {
            return new() {Data = {[0] = username}};
        }
    }
}
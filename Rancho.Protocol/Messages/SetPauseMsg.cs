using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class SetPauseMsg : Message
    {
        public override MessageType MessageType { get; } = MessageType.SetPause;
        public override dynamic[] Data { get; } = new dynamic[1];

        protected override void PackImpl()
        {
            Writer.WriteBoolean(Data[0]);
        }

        protected override bool Unpack(CborReader reader)
        {
            try
            {
                if (reader.PeekState() != CborReaderState.Boolean)
                {
                    return false;
                }
            }
            catch (CborContentException)
            {
                return false;
            }

            Data[0] = reader.ReadBoolean();

            return true;
        }

        public static SetPauseMsg Create(bool pause)
        {
            return new() {Data = {[0] = pause}};
        }
    }
}
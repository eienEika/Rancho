using System.Formats.Cbor;

namespace Rancho.Protocol.Messages
{
    public sealed class SetPauseMsgServer : Message
    {
        public override MessageType MessageType { get; } = MessageType.SetPauseServer;
        public override dynamic[] Data { get; } = new dynamic[1];

        private protected override void ReadData(CborReader reader)
        {
            Data[0] = reader.ReadBoolean();
        }

        private protected override void WriteData()
        {
            Writer.WriteBoolean(Data[0]);
        }

        public static SetPauseMsgServer Create(bool pause)
        {
            return new() {Data = {[0] = pause}};
        }
    }
}
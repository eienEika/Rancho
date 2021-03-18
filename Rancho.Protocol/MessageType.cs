namespace Rancho.Protocol
{
    public enum MessageType : ulong
    {
        None = 0,

        Hello = 17,
        ChatMessageClient = 345,
        SetUrlClient = 704,
        SetPauseClient = 1034,

        UserConnected = 34,
        ChatMessageServer = 456,
        SetUrlServer = 700,
        SetPauseServer = 1230,
    }
}
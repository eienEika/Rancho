namespace Rancho.Protocol
{
    public enum MessageType : ulong
    {
        None = 0,
        Connect = 17,
        ChatMessage = 345,
        SetUrl = 704,
        SetPause = 1034,
    }
}
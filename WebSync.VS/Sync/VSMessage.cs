using WebSync.VS.Sync;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class VSMessage
    {
        public VSMessageType Type { get; }
        public object Data { get; }

        public VSMessage(VSMessageType type, object data)
        {
            Type = type;
            Data = data;
        }
    }
}
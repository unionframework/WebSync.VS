using WebSync.VS.Sync;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class VSMessage
    {
        public VSMessageType Type { get; }
        public object Data { get; }
        public string asyncId { get; }

        public VSMessage(VSMessageType type, object data)
        {
            Type = type;
            Data = data;
        }

        public VSMessage(VSMessageType type, object data, string asyncId) : this(type, data)
        {
            this.asyncId = asyncId;
        }
    }
}
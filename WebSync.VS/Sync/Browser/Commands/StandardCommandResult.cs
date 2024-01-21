using Newtonsoft.Json;
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

    internal class VSErrorMessage : VSMessage
    {
        private readonly string Error;

        public VSErrorMessage(VSMessageType type, string error) : base(type, null)
        {
            Error = error;
        }
    }
}
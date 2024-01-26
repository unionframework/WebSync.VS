using WebSync.VS.Sync;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class VSErrorMessage : VSMessage
    {
        private readonly string Error;

        public VSErrorMessage(VSMessageType type, string error) : base(type, null)
        {
            Error = error;
        }
    }
}
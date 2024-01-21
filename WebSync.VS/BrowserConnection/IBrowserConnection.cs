using System;
using RoslynSpike.BrowserConnection.WebSocket;

namespace RoslynSpike.BrowserConnection
{
    /// <summary>
    /// Allows to send and receive data to SynchronizeIt browser extension
    /// </summary>
    internal interface IBrowserConnection
    {
        void Connect();
        void Close();
        bool Connected { get; }
        event EventHandler<VSMessage> Broadcasted;
        event EventHandler<BrowserMessage> BrowserMessageReceived;
        void Broadcast(VSMessage message);
    }
}
using System;
using System.Net.Sockets;
using Moq;
using NLog;
using NUnit.Framework;
using WebSocketSharp.Server;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class WebSocketBrowserConnection : IBrowserConnection
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private WebSocketServer _webSocketServer;
        private readonly int _serverPort;
        private readonly string _path;

        public event EventHandler<VSMessage> Broadcasted;
        public event EventHandler<BrowserMessage> BrowserMessageReceived;
        
        public bool Connected => Broadcasted != null && Broadcasted.GetInvocationList().Length > 0;

        public WebSocketBrowserConnection(int serverPort, string path)
        {
            _serverPort = serverPort;
            _path = path;
        }

        public void Connect() {
            if (_webSocketServer == null) {
                _webSocketServer = CreateSocketServer(_serverPort, _path);
            }
            try {
                _webSocketServer.Start();
            }
            catch (SocketException e) {
                _log.Error($@"Unable to start server {_webSocketServer.Address}", e);
            }
        }

        private WebSocketServer CreateSocketServer(int port, string path)
        {
            var webSocket = new WebSocketServer(port);
            webSocket.AddWebSocketService(path, () => new SyncBehaviour(this));
            return webSocket;
        }

        public void OnMessage(BrowserMessage message)
        {
            BrowserMessageReceived?.Invoke(this, message);
        }

        public void Close()
        {
            _webSocketServer.Stop();
        }

        public void Broadcast(VSMessage message)
        {
            Broadcasted?.Invoke(this, message);
        }
    }

    [TestFixture]
    public class WebSocketBrowserConnectionTest
    {
        private WebSocketBrowserConnection _connection;

        [SetUp]
        public void SetUp()
        {
            _connection = new WebSocketBrowserConnection(18488, "synchronize");
        }

        [Test]
        public void Connect()
        {
            // .Arrange
            // .Act
            _connection.Connect();
            // .Assert
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }
    }
}
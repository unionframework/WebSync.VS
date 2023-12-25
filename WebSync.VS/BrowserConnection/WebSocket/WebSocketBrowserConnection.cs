using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Moq;
using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RoslynSpike.Reflection;
using RoslynSpike.SessionWeb.Models;
using WebSocketSharp.Server;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    public class WebSocketBrowserConnection : IBrowserConnection
    {
        private static NLog.Logger _log = LogManager.GetCurrentClassLogger();

        private WebSocketServer _webSocketServer;
        private readonly int _serverPort;
        private readonly string _path;

        public event EventHandler<string> ProjectRequested;
        public event EventHandler ProjectNamesRequested;
        public event EventHandler<string> UrlToMatchReceived;
        public event EventHandler<SIMessage> Broadcasted;

        public IWebInfoSerializer Serializer { get; }
        

        public bool Connected => Broadcasted != null && Broadcasted.GetInvocationList().Length > 0;

        public WebSocketBrowserConnection(int serverPort, string path, IWebInfoSerializer serializer)
        {
            _serverPort = serverPort;
            _path = path;
            Serializer = serializer;
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
            webSocket.AddWebSocketService(path, () => new SynchronizeBehaviour(this));
            return webSocket;
        }

        public void OnMessage(SIMessage message)
        {
            switch (message.Type)
            {
                case SIMessageType.GetProjectNames:
                    OnProjectNamesRequested();
                    break;
                case SIMessageType.GetProject:
                    OnProjectRequested(message.Data);
                    break;
                case SIMessageType.MatchUrl:
                    OnMatchUrlReceived(message.Data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnProjectNamesRequested() => ProjectNamesRequested?.Invoke(this, EventArgs.Empty);

        private void OnProjectRequested(string projectName) => ProjectRequested?.Invoke(this, projectName);

        private void OnMatchUrlReceived(string url) => UrlToMatchReceived?.Invoke(this, url);

        public void SendProject(IEnumerable<IProjectInfo> webs) => OnBroadcasted(SIMessage.CreateProjectNamesMessage(Serializer.Serialize(webs)));

        public void SendUrlMatchResult(MatchUrlResult matchUrlResult) => OnBroadcasted(SIMessage.CreateUrlMatchResultessage(JsonConvert.SerializeObject(matchUrlResult)));

        public void Close()
        {
            _webSocketServer.Stop();
        }

        protected virtual void OnBroadcasted(SIMessage msg)
        {
            Broadcasted?.Invoke(this, msg);
        }

        public void SendProject(IProjectInfo projectInfo)
        {
            throw new NotImplementedException();
        }

        public void SendProjectNames(IEnumerable<string> projectNames)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class WebSocketBrowserConnectionTest
    {
        private WebSocketBrowserConnection _connection;

        [SetUp]
        public void SetUp()
        {
            var serializerMoq = new Mock<IWebInfoSerializer>();
            _connection = new WebSocketBrowserConnection(18488, "synchronize", serializerMoq.Object);
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
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
        public event EventHandler<BrowserMessage> Broadcasted;

        public IProjectInfoSerializer Serializer { get; }
        

        public bool Connected => Broadcasted != null && Broadcasted.GetInvocationList().Length > 0;

        public WebSocketBrowserConnection(int serverPort, string path, IProjectInfoSerializer serializer)
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

        public void OnMessage(BrowserMessage message)
        {
            // TODO: parse messages to typed objects
            switch (message.Type)
            {
                case BrowserMessageType.GetProjectNames:
                    OnProjectNamesRequested();
                    break;
                case BrowserMessageType.GetProject:
                    OnProjectRequested(message.Data as string);
                    break;
                case BrowserMessageType.MatchUrl:
                    OnMatchUrlReceived(message.Data as string);
                    break;
                case BrowserMessageType.OpenFile:
                    // TODO: open file matching the page in browser
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnProjectNamesRequested() => ProjectNamesRequested?.Invoke(this, EventArgs.Empty);

        private void OnProjectRequested(string projectName) => ProjectRequested?.Invoke(this, projectName);

        private void OnMatchUrlReceived(string url) => UrlToMatchReceived?.Invoke(this, url);

        public void SendUrlMatchResult(MatchUrlResult matchUrlResult) => OnBroadcasted(BrowserMessage.CreateUrlMatchResultessage(JsonConvert.SerializeObject(matchUrlResult)));

        public void Close()
        {
            _webSocketServer.Stop();
        }

        protected virtual void OnBroadcasted(BrowserMessage msg)
        {
            Broadcasted?.Invoke(this, msg);
        }

        public void SendProject(IProjectInfo projectInfo)
        {
            OnBroadcasted(BrowserMessage.CreateProjectMessage(Serializer.Serialize(projectInfo)));
        }

        public void SendProjectNames(IEnumerable<string> projectNames)
        {
            OnBroadcasted(BrowserMessage.CreateProjectNamesMessage(projectNames));
        }
    }

    [TestFixture]
    public class WebSocketBrowserConnectionTest
    {
        private WebSocketBrowserConnection _connection;

        [SetUp]
        public void SetUp()
        {
            var serializerMoq = new Mock<IProjectInfoSerializer>();
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
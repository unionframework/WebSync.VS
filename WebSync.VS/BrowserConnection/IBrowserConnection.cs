using System;
using System.Collections.Generic;
using RoslynSpike.Reflection;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection
{
    /// <summary>
    /// Allows to send and receive data to SynchronizeIt browser extension
    /// </summary>
    public interface IBrowserConnection
    {
        IWebInfoSerializer Serializer { get; }
        void Connect();
        void Close();
        bool Connected { get; }
        event EventHandler<string> ProjectRequested;
        event EventHandler ProjectNamesRequested;
        event EventHandler<string> UrlToMatchReceived;
        void SendProject(IProjectInfo projectInfo);
        void SendProjectNames(IEnumerable<string> projectNames);
        void SendUrlMatchResult(MatchUrlResult matchUrlResult);
    }
}
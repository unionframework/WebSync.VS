using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class UpdateWebsiteCommand : CommandWithDataBase<WebSiteMessage>
    {
        public UpdateWebsiteCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(WebSiteMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
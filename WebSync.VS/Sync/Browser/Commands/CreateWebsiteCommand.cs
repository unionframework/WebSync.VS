using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreateWebsiteCommand : ProjectCommandBase<WebSiteMessage>
    {
        public CreateWebsiteCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(WebSiteMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
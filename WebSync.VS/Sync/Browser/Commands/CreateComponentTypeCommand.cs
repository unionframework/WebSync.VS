using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreateComponentTypeCommand : ProjectCommandBase<ComponentTypeMessage>
    {
        public CreateComponentTypeCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(ComponentTypeMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
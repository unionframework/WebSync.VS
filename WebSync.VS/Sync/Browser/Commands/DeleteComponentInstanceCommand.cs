using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class DeleteComponentInstanceCommand : ProjectCommandBase<ComponentInstanceMessage>
    {
        public DeleteComponentInstanceCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(ComponentInstanceMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
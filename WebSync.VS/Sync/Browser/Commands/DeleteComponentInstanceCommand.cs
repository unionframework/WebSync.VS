using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class DeleteComponentInstanceCommand : CommandWithDataBase<ComponentInstanceMessage>
    {
        public DeleteComponentInstanceCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(ComponentInstanceMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
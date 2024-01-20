using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class DeleteComponentInstanceCommand : CommandBase
    {
        public DeleteComponentInstanceCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<StandardCommandResult> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreateComponentTypeCommand : CommandWithDataBase<ComponentTypeMessage>
    {
        public CreateComponentTypeCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(ComponentTypeMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class UpdateWebsiteCommand : CommandBase
    {
        public UpdateWebsiteCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
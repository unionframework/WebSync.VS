using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.IO;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class GetProjectsCommand : CommandBase
    {
        public GetProjectsCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<StandardCommandResult> ExecuteAsync()
        {
            var solutionName = Path.GetFileNameWithoutExtension(Solution.FilePath);
            return Task.FromResult(new StandardCommandResult());
            //_browserConnection.SendProjectNames(new List<string>() { solutionName });
        }
    }
}
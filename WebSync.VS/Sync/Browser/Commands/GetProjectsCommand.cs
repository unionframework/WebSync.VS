using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Collections.Generic;
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

        public override Task<VSMessage> ExecuteAsync()
        {
            var solutionName = Path.GetFileNameWithoutExtension(Solution.FilePath);
            return Task.FromResult(new VSMessage(VSMessageType.ProjectNames, new List<string>() { solutionName }));
        }
    }
}
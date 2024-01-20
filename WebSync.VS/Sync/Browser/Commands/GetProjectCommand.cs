using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class GetProjectCommand : CommandBase
    {
        private IProjectInfoPovider _projectInfoProvider;

        public GetProjectCommand(Solution solution, IProjectInfoPovider projectInfoProvider, object data) : base(solution, data)
        {
            _projectInfoProvider = projectInfoProvider;
        }

        public async override Task<StandardCommandResult> ExecuteAsync()
        {
            IEnumerable<IProjectInfo> projects = await _projectInfoProvider.GetProjectInfoAsync(false);
            return new StandardCommandResult();
        }
    }
}
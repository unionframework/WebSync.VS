using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class GetProjectCommand : CommandBase
    {
        private IProjectInfoPovider _projectInfoProvider;
        private IProjectInfoSerializer _serializer; 

        public GetProjectCommand(Solution solution, IProjectInfoPovider projectInfoProvider, IProjectInfoSerializer serializer, object data) : base(solution, data)
        {
            _projectInfoProvider = projectInfoProvider;
            _serializer = serializer;
        }

        public async override Task<VSMessage> ExecuteAsync()
        {
            IEnumerable<IProjectInfo> projects = await _projectInfoProvider.GetProjectInfoAsync(false);
            return new VSMessage(VSMessageType.Project, _serializer.Serialize(projects.First()));
        }
    }
}
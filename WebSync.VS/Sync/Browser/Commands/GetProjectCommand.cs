using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.ProjectInfo;

namespace WebSync.VS.Sync
{
    internal class GetProjectCommand : CommandWithDataBase<ProjectMessage>
    {
        private IProjectInfoPovider _projectInfoProvider;
        private IProjectInfoSerializer _serializer;
        private ProjectInfoCache _projectInfoCache;

        public GetProjectCommand(Solution solution, IProjectInfoPovider projectInfoProvider, IProjectInfoSerializer serializer, ProjectInfoCache projectInfoCache, object data) : base(solution, data)
        {
            _projectInfoProvider = projectInfoProvider;
            _serializer = serializer;
            _projectInfoCache = projectInfoCache;
        }

        public async override Task<VSMessage> ExecuteAsync(ProjectMessage message)
        {
            IProjectInfo project = await _projectInfoProvider.GetProjectInfoAsync(false);
            _projectInfoCache.StoreProjectInfo(project);
            return new VSMessage(VSMessageType.Project, _serializer.Serialize(project));
        }
    }
}
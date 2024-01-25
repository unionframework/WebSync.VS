using Microsoft.CodeAnalysis;
using NLog;
using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.Compiler;
using RoslynSpike.SessionWeb;
using WebSync.VS.ProjectInfo;
using WebSync.VS.Sync;
using WebSync.VS.Sync.Workspace;

namespace WebSync.VS
{
    internal class SyncMediator
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Workspace _workspace;
        private readonly IBrowserConnection _browserConnection;
        private readonly IProjectInfoPovider _projectInfoProvider;
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly ProjectInfoCache _projectInfoCache;
        private readonly IProjectInfoSerializer _projectInfoSerializer;
        private BrowserMessagesHandler _messagesHandler;
        private WorkspaceUpdatesHandler _workspaceUpdatesHandler;

        public SyncMediator(Workspace workspace, 
            IBrowserConnection browserConnection,
            IProjectInfoPovider projectInfoProvider, 
            ProjectInfoCache projectInfoCache,
            IProjectInfoSerializer projectInfoSerializer,
            IAssemblyProvider assemblyProvider)
        {
            _workspace = workspace;
            _browserConnection = browserConnection;
            _projectInfoProvider = projectInfoProvider;
            _projectInfoSerializer = projectInfoSerializer;
            _assemblyProvider = assemblyProvider;
            _projectInfoCache = projectInfoCache;
            _messagesHandler = new BrowserMessagesHandler(_workspace, _assemblyProvider, _projectInfoProvider, _projectInfoCache, _projectInfoSerializer);
            _workspaceUpdatesHandler = new WorkspaceUpdatesHandler();

            _browserConnection.BrowserMessageReceived += _browserConnection_BrowserMessageReceived;
            _workspace.WorkspaceChanged += _workspace_WorkspaceChanged;
        }

        private async void _browserConnection_BrowserMessageReceived(object sender, BrowserMessage e)
        {
            var responseMessage = await _messagesHandler.HandleAsync(e);
            if (responseMessage != null)
            {
                _browserConnection.Broadcast(responseMessage);
            }
        }

        private void _workspace_WorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            //            if (e.Kind == WorkspaceChangeKind.ProjectAdded) {
            //                MatchUrl("http://10.51.27.92/km/admin/UserInterface/General");
            //            }
            if (!_browserConnection.Connected)
            {
                return;
            }

            // TODO: how to handle other events
            if (e.Kind == WorkspaceChangeKind.DocumentChanged)
            {
                CollectAndSynchronizeChanges(e.DocumentId);
            }
        }

        //private void _browserConnection_ProjectNamesRequested(object sender, EventArgs e)
        //{
        //    var solutionName = Path.GetFileNameWithoutExtension(_workspace.CurrentSolution.FilePath);
        //    _browserConnection.SendProjectNames(new List<string>() { solutionName });
        //}

        //private void _browserConnection_ProjectRequested(object sender, string e)
        //{
        //    CollectAndSynchronizeChanges();
        //}

        //private void _browserConnection_UrlToMatchReceived(object sender, string url)
        //{
        //    MatchUrl(url);
        //}

        private async void CollectAndSynchronizeChanges(DocumentId documentId)
        {
            var project = _workspace.CurrentSolution.GetProject(documentId.ProjectId);
            var projectInfo = _projectInfoCache.GetProjectInfo(project.Name);
            if (projectInfo != null)
            {
                if (await _projectInfoProvider.UpdateProjectsAsync(projectInfo, documentId))
                {
                    _browserConnection.Broadcast(new VSMessage(VSMessageType.Project, _projectInfoSerializer.Serialize(projectInfo)));
                }
            }
        }
    }
}

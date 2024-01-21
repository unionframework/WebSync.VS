using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NLog;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Compiler;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;
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
        private IEnumerable<IProjectInfo> _projects;
        private BrowserMessagesHandler _messagesHandler;
        private WorkspaceUpdatesHandler _workspaceUpdatesHandler;

        public SyncMediator(Workspace workspace, IBrowserConnection browserConnection,
            IProjectInfoPovider projectInfoProvider, IAssemblyProvider assemblyProvider)
        {
            _workspace = workspace;
            _browserConnection = browserConnection;
            _projectInfoProvider = projectInfoProvider;
            _assemblyProvider = assemblyProvider;
            _messagesHandler = new BrowserMessagesHandler(_workspace.CurrentSolution, _assemblyProvider, _projectInfoProvider);
            _workspaceUpdatesHandler = new WorkspaceUpdatesHandler();

            _browserConnection.BrowserMessageReceived += _browserConnection_BrowserMessageReceived;
            _workspace.WorkspaceChanged += _workspace_WorkspaceChanged;
        }

        private async void _browserConnection_BrowserMessageReceived(object sender, BrowserMessage e)
        {
            var vsMessage = await _messagesHandler.HandleAsync(e);
            _browserConnection.Broadcast(vsMessage);
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
            if (_projects!=null && _projects.Count() > 0)
            {
                if (await _projectInfoProvider.UpdateProjectsAsync(_projects.First(), documentId))
                {
                    SynchronizeProjectInfo(_projects);
                }
            }
        }

        //private async void CollectAndSynchronizeChanges()
        //{
        //    IEnumerable<IProjectInfo> projects = await _projectInfoProvider.GetProjectInfoAsync(false);
        //    SynchronizeProjectInfo(projects);
        //}

        private void SynchronizeProjectInfo(IEnumerable<IProjectInfo> projects)
        {
            //var pageType = sessionWebs.First().PageTypes["km.tests.selenium.services.kmNewUI.Pages.EndUser.Search.SearchPageBase"];
            // . Currently, there is only one
            //_browserConnection.SendProject(projects.First());
            _projects = projects;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.CodeAnalysis;
using NLog;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Compiler;
using RoslynSpike.Reflection;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;

namespace WebSync.VS
{
    public class WebSync
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Workspace _workspace;
        private readonly IBrowserConnection _browserConnection;
        private readonly IProjectInfoPovider _projectInfoProvider;
        private readonly IAssemblyProvider _assemblyProvider;

        private IEnumerable<IProjectInfo> _projects;


        public WebSync(Workspace workspace, IBrowserConnection browserConnection,
            IProjectInfoPovider sessionWebProvider, IAssemblyProvider assemblyProvider)
        {
            _workspace = workspace;
            _browserConnection = browserConnection;
            _projectInfoProvider = sessionWebProvider;
            _assemblyProvider = assemblyProvider;
            _browserConnection.ProjectNamesRequested += _browserConnection_ProjectNamesRequested;
            _browserConnection.ProjectRequested += _browserConnection_ProjectRequested;
            _browserConnection.UrlToMatchReceived += _browserConnection_UrlToMatchReceived;
            _workspace.WorkspaceChanged += _workspace_WorkspaceChanged;
        }

        private void _browserConnection_ProjectNamesRequested(object sender, EventArgs e)
        {
            var solutionName = Path.GetFileNameWithoutExtension(_workspace.CurrentSolution.FilePath);
            _browserConnection.SendProjectNames(new List<string>() { solutionName });
        }

        private void _browserConnection_ProjectRequested(object sender, string e)
        {
            CollectAndSynchronizeChanges();
        }

        private void _browserConnection_UrlToMatchReceived(object sender, string url)
        {
            MatchUrl(url);
        }

        private void MatchUrl(string url)
        {
            try
            {
                LogInfo($"MatchUrl: {url}");
                _log.Info($"MatchUrl: {url}");
                var assemblies = _assemblyProvider.GetAssemblies();
                if (assemblies != null)
                {
                    LogInfo($"Compiled successfully.");
                    var urlMatcher = new UrlMatcher(assemblies.Item1, assemblies.Item2);
                    var matchUrlResult = urlMatcher.Match(url);
                    LogInfo($"Matched successfully: {matchUrlResult.ServiceId}-{matchUrlResult.PageId}");
                    _browserConnection.SendUrlMatchResult(matchUrlResult);
                    if (!string.IsNullOrWhiteSpace(matchUrlResult.PageId))
                    {
                        OpenDocumentWithType(matchUrlResult.PageId);
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo($"Error!!!");
                LogInfo(e.Message);
                LogInfo(e.StackTrace);
                if (e.InnerException != null)
                {
                    LogInfo(e.InnerException.Message);
                    LogInfo(e.InnerException.StackTrace);
                }
            }
        }

        private void OpenDocumentWithType(string typeMetadataName)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                foreach (var project in _workspace.CurrentSolution.Projects)
                {
                    var typeByMetadataName =
                        project.GetCompilationAsync().Result.GetTypeByMetadataName(typeMetadataName);
                    if (typeByMetadataName != null)
                    {
                        var location = typeByMetadataName.Locations.First();
                        var documentFilePath = location.SourceTree.FilePath;
                        var documentIdsWithFilePath =
                            _workspace.CurrentSolution.GetDocumentIdsWithFilePath(documentFilePath);
                        if (documentIdsWithFilePath.Length > 0)
                        {
                            _workspace.OpenDocument(documentIdsWithFilePath.First());
                            break;
                        }
                    }
                }
            });

        }

        private void LogInfo(string s)
        {
            //            using (var streamWriter = File.AppendText("c:\\logs\\natu.log")) {
            //                streamWriter.WriteLine(s);
            //            }
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

        private async void CollectAndSynchronizeChanges()
        {
            IEnumerable<IProjectInfo> projects = await _projectInfoProvider.GetProjectInfoAsync(false);
            SynchronizeProjectInfo(projects);
        }

        private void SynchronizeProjectInfo(IEnumerable<IProjectInfo> projects)
        {
            //var pageType = sessionWebs.First().PageTypes["km.tests.selenium.services.kmNewUI.Pages.EndUser.Search.SearchPageBase"];
            // . Currently, there is only one
            _browserConnection.SendProject(projects.First());
            _projects = projects;
        }
    }
}

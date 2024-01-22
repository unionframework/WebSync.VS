using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    public class ProjectMessage
    {
        public string projectName;
    }

    public class OpenFileMessage:ProjectMessage
    {
        public string fullClassName;
    }

    internal class OpenFileForClassCommand : CommandBase
    {
        private Microsoft.CodeAnalysis.Workspace _workspace;

        public OpenFileForClassCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace.CurrentSolution, data)
        {
            _workspace = workspace;
        }

        public override Task<VSMessage> ExecuteAsync()
        {
            OpenFileMessage openFileMessage = JsonConvert.DeserializeObject<OpenFileMessage>(JsonConvert.SerializeObject(Data));
            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                foreach (var project in _workspace.CurrentSolution.Projects)
                {
                    var typeByMetadataName =
                        project.GetCompilationAsync().Result.GetTypeByMetadataName(openFileMessage.fullClassName);
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
            return Task.FromResult((VSMessage)null);
        }
    }
}
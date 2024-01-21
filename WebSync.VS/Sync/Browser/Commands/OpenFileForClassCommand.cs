using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace WebSync.VS.Sync
{
    internal class OpenFileForClassCommand : CommandBase
    {
        public OpenFileForClassCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }

        //private void OpenDocumentWithType(string typeMetadataName)
        //{
        //    Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
        //    {
        //        foreach (var project in _workspace.CurrentSolution.Projects)
        //        {
        //            var typeByMetadataName =
        //                project.GetCompilationAsync().Result.GetTypeByMetadataName(typeMetadataName);
        //            if (typeByMetadataName != null)
        //            {
        //                var location = typeByMetadataName.Locations.First();
        //                var documentFilePath = location.SourceTree.FilePath;
        //                var documentIdsWithFilePath =
        //                    _workspace.CurrentSolution.GetDocumentIdsWithFilePath(documentFilePath);
        //                if (documentIdsWithFilePath.Length > 0)
        //                {
        //                    _workspace.OpenDocument(documentIdsWithFilePath.First());
        //                    break;
        //                }
        //            }
        //        }
        //    });

        //}
    }
}
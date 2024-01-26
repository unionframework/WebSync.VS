using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    // https://roslynquoter.azurewebsites.net/
    internal class CreatePageTypeCommand : ProjectCommandBase<PageTypeMessage>
    {
        public CreatePageTypeCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(PageTypeMessage message)
        {
            var project = GetProject(message.projectName);

            var pageFileName = $"{message.name}.cs";
            var pageSyntax = UnionSyntaxFactory.GeneratePage(message.name, message.absoluteUrl);
            Document newDocument = project.AddDocument(pageFileName, pageSyntax);

            ApplyChanges(newDocument.Project.Solution);
            return Task.FromResult(null as VSMessage);
        }

    }
}
using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreateWebsiteCommand : ProjectCommandBase<WebSiteMessage>
    {
        public CreateWebsiteCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync(WebSiteMessage message)
        {
            var project = GetProject(message.projectName);

            var basePageName = $"{message.name}Page";
            var basePageFileName = $"{basePageName}.cs";
            var webSiteFileName = $"{message.name}.cs";

            var webSiteSyntax = UnionSyntaxFactory.GenerateWebSite(message.name, basePageName, message.baseUrl);
            var basePageSyntax = UnionSyntaxFactory.GenerateBasePage(basePageName);

            Document newDocument = project.AddDocument(webSiteFileName, webSiteSyntax);
            newDocument = newDocument.Project.AddDocument(basePageFileName, basePageSyntax);

            ApplyChanges(newDocument.Project.Solution);
            return Task.FromResult(null as VSMessage);
        }
    }
}
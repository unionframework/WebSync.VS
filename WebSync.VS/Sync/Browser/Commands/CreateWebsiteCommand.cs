using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreateWebsiteCommand : ProjectCommandBase<WebSiteMessage>
    {
        public string AsyncId;
        public CreateWebsiteCommand(Microsoft.CodeAnalysis.Workspace workspace, object data, string asyncId) : base(workspace, data)
        {
            AsyncId = asyncId;
        }

        public override Task<VSMessage> ExecuteAsync(WebSiteMessage message)
        {
            var project = GetProject(message.projectName);

            var basePageName = $"{message.name}PageBase";
            var basePageFileName = $"{basePageName}.cs";
            var webSiteFileName = $"{message.name}.cs";

            var webSiteSyntax = UnionSyntaxFactory.GenerateWebSite(message.name, basePageName, message.baseUrl);
            var basePageSyntax = UnionSyntaxFactory.GenerateBasePage(basePageName);

            Document newDocument = project.AddDocument(webSiteFileName, webSiteSyntax);
            newDocument = newDocument.Project.AddDocument(basePageFileName, basePageSyntax);

            ApplyChanges(newDocument.Project.Solution);
            return Task.FromResult(new VSMessage(VSMessageType.CreateWebsiteResponse, null, AsyncId));
        }
    }
}
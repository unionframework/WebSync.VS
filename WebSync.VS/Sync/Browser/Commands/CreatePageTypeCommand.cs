using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;

namespace WebSync.VS.Sync
{
    internal class CreatePageTypeCommand : ProjectCommandBase<PageTypeMessage>
    {
        public CreatePageTypeCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override async Task<VSMessage> ExecuteAsync(PageTypeMessage message)
        {
            var project = GetProject(message.projectName);
            var compilation = await project.GetCompilationAsync();

            var pageFileName = $"{message.pageType.id}.cs";
            Document newDocument = project.AddDocument(pageFileName, "<html></html>");

            //var containerType = compilation.GetTypeByMetadataName(message.componentInstance.parentId);
            //var member = containerType.GetMembers().SingleOrDefault(m => m.Name == message.componentInstance.name);
            //var attribute = member.GetAttributeOfType(ReflectionNames.AUTOINIT_ATTRRIBUTE);
            //var attributeSyntax = await attribute.ApplicationSyntaxReference.GetSyntaxAsync();

            //var newArgumentSyntax = AttributeArgument(
            //    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(newSelector)));
            //var argumentSyntax = attributeSyntax.DescendantNodes().OfType<AttributeArgumentSyntax>().First();
            //var rootSyntax = await attributeSyntax.SyntaxTree.GetRootAsync();
            //var newRootSyntax = rootSyntax.ReplaceNode(argumentSyntax, newArgumentSyntax);

            //var document = project.GetDocument(attributeSyntax.SyntaxTree);
            //var text = newRootSyntax.GetText();
            //var newDocument = document.WithText(newRootSyntax.GetText());

            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = Workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }
    }
}
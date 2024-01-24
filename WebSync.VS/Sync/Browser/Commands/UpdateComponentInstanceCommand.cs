using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.Ember.DTO;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace WebSync.VS.Sync
{
    public class UpdateComponentInstanceMessage : ProjectMessage
    {
        public ComponentInstanceDto componentInstance;
    }

    internal class UpdateComponentInstanceCommand : CommandBase
    {
        public UpdateComponentInstanceCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public async override Task<VSMessage> ExecuteAsync()
        {
            var message = JsonConvert.DeserializeObject<UpdateComponentInstanceMessage>(JsonConvert.SerializeObject(Data));
            var project = Solution.Projects.FirstOrDefault(p => p.Name == message.projectName);
            if (project == null)
            {
                throw new InvalidOperationException($"Project not found: '{project.Name}'");
            }
            var compilation = await project.GetCompilationAsync();
            var containerType = compilation.GetTypeByMetadataName(message.componentInstance.parentId);
            var member = containerType.GetMembers().SingleOrDefault(m => m.Name == message.componentInstance.name);
            var attribute = member.GetAttributeOfType(ReflectionNames.AUTOINIT_ATTRRIBUTE);
            var attributeSyntax = await attribute.ApplicationSyntaxReference.GetSyntaxAsync();

            var newArgumentSyntax = AttributeArgument(
                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(".updated")));
            var argumentSyntax = attributeSyntax.DescendantNodes().OfType<AttributeArgumentSyntax>().First();
            var rootSyntax = await attributeSyntax.SyntaxTree.GetRootAsync();
            var newRootSyntax = rootSyntax.ReplaceNode(argumentSyntax, newArgumentSyntax);

            var document = project.GetDocument(attributeSyntax.SyntaxTree);
            var newDocument = document.WithText(newRootSyntax.GetText());

            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                Solution.Workspace.CanApplyChange(ApplyChangesKind.ChangeDocument);
                var updated = Solution.Workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }
    }
}
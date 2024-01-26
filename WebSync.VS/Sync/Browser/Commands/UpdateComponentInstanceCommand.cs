using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection.WebSocket;
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
    internal class UpdateComponentInstanceCommand : CommandWithDataBase<ComponentInstanceMessage>
    {
        Microsoft.CodeAnalysis.Workspace _workspace;

        public UpdateComponentInstanceCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace.CurrentSolution, data)
        {
            _workspace = workspace;
        }

        public async override Task<VSMessage> ExecuteAsync(ComponentInstanceMessage message)
        {
            var newSelector = message.componentInstance.fieldName; //message.componentInstance.initializationAttribute.constructorArguments.First().ToString();
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
                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(newSelector)));
            var argumentSyntax = attributeSyntax.DescendantNodes().OfType<AttributeArgumentSyntax>().First();
            var rootSyntax = await attributeSyntax.SyntaxTree.GetRootAsync();
            var newRootSyntax = rootSyntax.ReplaceNode(argumentSyntax, newArgumentSyntax);

            var document = project.GetDocument(attributeSyntax.SyntaxTree);
            var text = newRootSyntax.GetText();
            var newDocument = document.WithText(newRootSyntax.GetText());

            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = _workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }
    }
}
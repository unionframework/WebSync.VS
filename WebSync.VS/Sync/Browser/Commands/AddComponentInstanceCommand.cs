using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.Sync;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal abstract class ProjectCommandBase : CommandBase
    {
        protected ProjectCommandBase(Solution solution, object data) : base(solution, data)
        {
        }

        protected Project GetProject(string name) {
            var project = Solution.Projects.FirstOrDefault(p => p.Name == name);
            if (project == null)
            {
                throw new InvalidOperationException($"Project not found: '{project.Name}'");
            }
            return project;
        }
    }

    internal class AddComponentInstanceCommand : ProjectCommandBase
    {
        private Workspace _workspace;

        public AddComponentInstanceCommand(Workspace workspace, object data) : base(workspace.CurrentSolution, data)
        {
            _workspace = workspace;
        }

        public async override Task<VSMessage> ExecuteAsync()
        {
            var message = JsonConvert.DeserializeObject<ComponentInstanceMessage>(JsonConvert.SerializeObject(Data));

            var project = GetProject(message.projectName);
            var compilation = await project.GetCompilationAsync();
            var containerType = compilation.GetTypeByMetadataName(message.componentInstance.parentId);

            var lastElementMember = GetLastElementMember(containerType);
            var lastElementMemberSyntax = await lastElementMember.DeclaringSyntaxReferences.First().GetSyntaxAsync();

            var newElementFieldSyntax = GenerateElementField(
                message.componentInstance.fieldName, 
                message.componentInstance.componentTypeId, 
                message.componentInstance.initializationAttribute.constructorArguments.First().ToString());
            
            var rootSyntax = await lastElementMemberSyntax.SyntaxTree.GetRootAsync();
            var newRootSyntax = rootSyntax.InsertNodesAfter(lastElementMemberSyntax, new[] { newElementFieldSyntax });

            var document = project.GetDocument(lastElementMemberSyntax.SyntaxTree);
            var newDocument = document.WithText(newRootSyntax.GetText());

            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = _workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }

        private CompilationUnitSyntax GenerateElementField(string fieldName, string componentTypeId, string selector)
        {
            return CompilationUnit()
                .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    GlobalStatement(
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                IdentifierName(componentTypeId))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier(fieldName)))))
                        .WithAttributeLists(
                            SingletonList<AttributeListSyntax>(
                                AttributeList(
                                    SingletonSeparatedList<AttributeSyntax>(
                                        Attribute(
                                            IdentifierName(ReflectionNames.AUTOINIT_ATTRRIBUTE))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList<AttributeArgumentSyntax>(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.CharacterLiteralExpression,
                                                            Literal(selector))))))))))
                        .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PublicKeyword))))))
            .NormalizeWhitespace();
        }

        private ISymbol GetLastElementMember(INamedTypeSymbol containerType) =>
            containerType.GetMembers()
                .Reverse()
                .Where(m => m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property)
                .First();
    }
}

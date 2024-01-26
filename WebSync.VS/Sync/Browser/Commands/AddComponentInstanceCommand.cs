using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.SessionWeb;
using System.Linq;
using System.Threading.Tasks;
using WebSync.VS.Sync;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal class AddComponentInstanceCommand : ProjectCommandBase<ComponentInstanceMessage>
    {
        private Workspace _workspace;

        public AddComponentInstanceCommand(Workspace workspace, object data) : base(workspace, data)
        {
            _workspace = workspace;
        }

        public async override Task<VSMessage> ExecuteAsync(ComponentInstanceMessage message)
        {
            var project = GetProject(message.projectName);
            var compilation = await project.GetCompilationAsync();
            var containerType = compilation.GetTypeByMetadataName(message.componentInstance.parentId);

            var typeSyntax = await containerType.DeclaringSyntaxReferences.First().GetSyntaxAsync();
            var lastFieldSyntax = GetLastElementMember(typeSyntax);

            var newElementFieldSyntax = GenerateElementField(
                message.componentInstance.fieldName, 
                message.componentInstance.componentTypeId, 
                message.componentInstance.initializationAttribute.constructorArguments.First().ToString());
            
            var rootSyntax = await lastFieldSyntax.SyntaxTree.GetRootAsync();
            var newRootSyntax = rootSyntax.InsertNodesAfter(lastFieldSyntax, new[] { newElementFieldSyntax });

            var document = project.GetDocument(lastFieldSyntax.SyntaxTree);
            var newDocument = document.WithText(newRootSyntax.GetText());

            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = _workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }

        private FieldDeclarationSyntax GenerateElementField(string fieldName, string componentTypeId, string selector)
        {
            return FieldDeclaration(
                    VariableDeclaration(
                        IdentifierName(Identifier(
                                TriviaList(),
                                componentTypeId,
                                TriviaList(
                                    Space)))
                        )
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
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal(selector))))))))
                        .WithOpenBracketToken(
                            Token(
                                TriviaList(
                                    new[]{
                                        CarriageReturnLineFeed,
                                        Whitespace("        ")}),
                                SyntaxKind.OpenBracketToken,
                                TriviaList()))
                        .WithCloseBracketToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.CloseBracketToken,
                                TriviaList(
                                    CarriageReturnLineFeed)))))
                .WithModifiers(
                    TokenList(
                        Token(
                            TriviaList(
                                Whitespace("        ")),
                            SyntaxKind.PublicKeyword,
                            TriviaList(
                                Space))))
                .WithSemicolonToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.SemicolonToken,
                        TriviaList(
                            CarriageReturnLineFeed)));
        }

        private SyntaxNode GetLastElementMember(SyntaxNode typeSyntax) =>
             typeSyntax.DescendantNodes().OfType<FieldDeclarationSyntax>().Last();
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync.Browser.Messages;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace WebSync.VS.Sync
{
    // https://roslynquoter.azurewebsites.net/
    internal class CreatePageTypeCommand : ProjectCommandBase<PageTypeMessage>
    {
        public CreatePageTypeCommand(Microsoft.CodeAnalysis.Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override async Task<VSMessage> ExecuteAsync(PageTypeMessage message)
        {
            var project = GetProject(message.projectName);
            var compilation = await project.GetCompilationAsync();

            var pageFileName = $"{message.name}.cs";
            var pageSyntax = GeneratePage(message.name, message.absoluteUrl);
            Document newDocument = project.AddDocument(pageFileName, pageSyntax);
            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = Workspace.TryApplyChanges(newDocument.Project.Solution);
            });
            return null;
        }

        private CompilationUnitSyntax GeneratePage(string pageName, string absolutePath)
        {
            return CompilationUnit()
                .WithUsings(
                    List<UsingDirectiveSyntax>(
                        new UsingDirectiveSyntax[]{
                            UsingDirective(
                                IdentifierName("Union"))
                            .WithUsingKeyword(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList(
                                        Space)))
                            .WithSemicolonToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.SemicolonToken,
                                    TriviaList(
                                        CarriageReturnLineFeed))),
                            UsingDirective(
                                QualifiedName(
                                    IdentifierName("Union"),
                                    IdentifierName("Attributes")))
                            .WithUsingKeyword(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList(
                                        Space)))
                            .WithSemicolonToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.SemicolonToken,
                                    TriviaList(
                                        CarriageReturnLineFeed)))}))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                                IdentifierName(
                                    Identifier(
                                        TriviaList(),
                                        "WebSync",
                                        TriviaList(CarriageReturnLineFeed))))
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    CarriageReturnLineFeed),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList(
                                    Space)))
                        .WithOpenBraceToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.OpenBraceToken,
                                TriviaList(
                                    CarriageReturnLineFeed)))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration(
                                    Identifier(
                                        TriviaList(),
                                        pageName,
                                        TriviaList(
                                            Space)))
                                .WithModifiers(
                                    TokenList(
                                        Token(
                                            TriviaList(
                                                Whitespace("    ")),
                                            SyntaxKind.InternalKeyword,
                                            TriviaList(
                                                Space))))
                                .WithKeyword(
                                    Token(
                                        TriviaList(),
                                        SyntaxKind.ClassKeyword,
                                        TriviaList(
                                            Space)))
                                .WithBaseList(
                                    BaseList(
                                        SingletonSeparatedList<BaseTypeSyntax>(
                                            SimpleBaseType(
                                                IdentifierName(
                                                    Identifier(
                                                        TriviaList(),
                                                        "UnionPage",
                                                        TriviaList(
                                                            CarriageReturnLineFeed))))))
                                    .WithColonToken(
                                        Token(
                                            TriviaList(),
                                            SyntaxKind.ColonToken,
                                            TriviaList(
                                                Space))))
                                .WithOpenBraceToken(
                                    Token(
                                        TriviaList(
                                            Whitespace("    ")),
                                        SyntaxKind.OpenBraceToken,
                                        TriviaList(
                                            CarriageReturnLineFeed)))
                                .WithMembers(
                                    SingletonList<MemberDeclarationSyntax>(
                                        PropertyDeclaration(
                                            PredefinedType(
                                                Token(
                                                    TriviaList(),
                                                    SyntaxKind.StringKeyword,
                                                    TriviaList(
                                                        Space))),
                                            Identifier(
                                                TriviaList(),
                                                "AbsolutePath",
                                                TriviaList(
                                                    Space)))
                                        .WithModifiers(
                                            TokenList(
                                                new[]{
                                                    Token(
                                                        TriviaList(
                                                            Whitespace("        ")),
                                                        SyntaxKind.PublicKeyword,
                                                        TriviaList(
                                                            Space)),
                                                    Token(
                                                        TriviaList(),
                                                        SyntaxKind.OverrideKeyword,
                                                        TriviaList(
                                                            Space))}))
                                        .WithExpressionBody(
                                            ArrowExpressionClause(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal(absolutePath)))
                                            .WithArrowToken(
                                                Token(
                                                    TriviaList(),
                                                    SyntaxKind.EqualsGreaterThanToken,
                                                    TriviaList(
                                                        Space))))
                                        .WithSemicolonToken(
                                            Token(
                                                TriviaList(),
                                                SyntaxKind.SemicolonToken,
                                                TriviaList(
                                                    CarriageReturnLineFeed)))))
                                .WithCloseBraceToken(
                                    Token(
                                        TriviaList(
                                            Whitespace("    ")),
                                        SyntaxKind.CloseBraceToken,
                                        TriviaList(
                                            CarriageReturnLineFeed)))))));
        }
    }
}
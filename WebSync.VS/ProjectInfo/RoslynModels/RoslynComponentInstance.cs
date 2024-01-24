using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.VisualStudio.Package;
using RoslynSpike.SessionWeb.Models;
using Microsoft.VisualStudio.PlatformUI;

namespace RoslynSpike.SessionWeb.RoslynModels
{
    //https://github.com/code-cracker/code-cracker/blob/master/src/CSharp/CodeCracker/Style/ConsoleWriteLineCodeFixProvider.cs
    //https://joshvarty.com/2015/08/18/learn-roslyn-now-part-12-the-documenteditor/

    public class RoslynComponentInstance : RoslynComponentInstanceBase<IComponentInstance>, IComponentInstance {
        public RoslynComponentInstance(string parentId, ISymbol field, AttributeData attr)
            : base(parentId, field, attr) {
        }

        public override void SynchronizeTo(IComponentInstance model)
        {
            //string sourceCode = "public class Analog { public const string Value = \"UV254\"; public const string Description = \"A Description\";}";

            //SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            //CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();

            //LiteralExpressionSyntax afterLiteralExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("UV220"));

            //LiteralExpressionSyntax beforeLiteralExpressionSyntax = null;
            //foreach (VariableDeclarationSyntax variableDeclarationSyntax in syntaxRoot.DescendantNodes().OfType<VariableDeclarationSyntax>())
            //{
            //    foreach (VariableDeclaratorSyntax variableDeclaratorSyntax in variableDeclarationSyntax.Variables)
            //    {
            //        if (variableDeclaratorSyntax.Identifier.ValueText == "Value")
            //        {
            //            beforeLiteralExpressionSyntax = variableDeclaratorSyntax.DescendantNodes().OfType<LiteralExpressionSyntax>().Single();
            //            break;
            //        }
            //    }
            //    if (beforeLiteralExpressionSyntax != null)
            //    {
            //        break;
            //    }
            //}
            //var newRoot = syntaxRoot.ReplaceNode(beforeLiteralExpressionSyntax, afterLiteralExpressionSyntax);
            //var fixedTree = newRoot.SyntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
        }
    }
}
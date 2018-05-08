using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class MethodBodyGetter
    {
        public BlockSyntax GetBodyFor(MethodDeclarationSyntax methodDeclaration)
        {
            return SyntaxFactory.Block();
        }
    }
}

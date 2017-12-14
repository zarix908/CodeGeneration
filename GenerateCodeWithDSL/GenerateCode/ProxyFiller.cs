using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenerateCode
{
    public class ProxyFiller
    {
        public BlockSyntax GetMethodBody(string name, List<Tuple<string, string>> parameters)
        {
            var firstArgumentName = parameters[0].Item1;
            var secondArgumentName = parameters[1].Item1;

            var convertSecondArgExpression = GenerateConvertExpression(secondArgumentName);

            var addExpression = SyntaxFactory.BinaryExpression(SyntaxKind.AddExpression,
                SyntaxFactory.IdentifierName(firstArgumentName), convertSecondArgExpression);

            var returnStatement = SyntaxFactory.ReturnStatement(addExpression);

            return SyntaxFactory.Block(returnStatement);
        }

        private InvocationExpressionSyntax GenerateConvertExpression(string argumentName)
        {
            var arguments = SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(argumentName)
                    )));

            return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("Convert"),
                    SyntaxFactory.IdentifierName("ToString"))
                .WithOperatorToken(
                    SyntaxFactory.Token(
                        SyntaxKind.DotToken)),
                        arguments);
        }
    }
}

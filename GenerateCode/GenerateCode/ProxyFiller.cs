using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenerateCode
{
    public class ProxyFiller
    {
        public BlockSyntax GetMethodBody(string name, List<Tuple<string, string>> parameters)
        {
            var firstVariableNmae = parameters[0].Item1;
            var secondVariableName = parameters[1].Item1;

            var statement = new StringBuilder();
            statement.Append("return ");
            statement.Append(firstVariableNmae + ".ToString()");
            statement.Append("+");
            statement.Append(secondVariableName + ".ToString();");

            var expressionSyntax = SyntaxFactory.ParseStatement(statement.ToString());
            return SyntaxFactory.Block(expressionSyntax);
        }
    }
}

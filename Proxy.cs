using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class Proxy
    {
        public IEnumerable<CompilationUnitSyntax> NamespaceDeclarations { get; }

        public Proxy(IEnumerable<CompilationUnitSyntax> namespaceDeclarations)
        {
            NamespaceDeclarations = namespaceDeclarations;
        }
    }
}

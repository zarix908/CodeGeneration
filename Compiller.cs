using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class Compiller
    {
        public void Compile(Proxy proxy)
        {
            var compilationTrees = CollectCompilationUnitsFrom(proxy.NamespaceUnits)
                .Select(el => SyntaxFactory.SyntaxTree(el));
            var enumerator = compilationTrees.GetEnumerator();

            for (int i = 0; i < compilationTrees.Count(); i++)
            {
                enumerator.MoveNext();
                SaveToFile(enumerator.Current, i);
            }

            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                compilationTrees,
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var result = compilation.Emit("Test.dll");

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                {
                    Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }
            }
            else
                Console.WriteLine("Success!");
        }

        private void SaveToFile(SyntaxTree tree, int id)
        {
            var unit = tree.GetRoot();
            var outputFile = new StreamWriter($"files/{id}.txt");
            outputFile.Write(unit.NormalizeWhitespace());
            outputFile.Flush();
        }

        private IEnumerable<CompilationUnitSyntax> CollectCompilationUnitsFrom(
            IEnumerable<NamespaceUnit> namespaceUnits)
        {
            foreach (var namespaceUnit in namespaceUnits)
            {
                var namespaceDeclaration = namespaceUnit.NamespaceDeclaration;
                var usings = SyntaxFactory.List(namespaceUnit.Usings);

                var compilationUnit = SyntaxFactory.CompilationUnit()
                    .AddMembers(namespaceDeclaration)
                    .WithUsings(usings);

                yield return compilationUnit;
            }
        }
    }
}

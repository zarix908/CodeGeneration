using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class LibraryProxyGenerator
    {
        private readonly ClassProxyGenerator classProxyGenerator;

        public LibraryProxyGenerator(ClassProxyGenerator classProxyGenerator)
        {
            this.classProxyGenerator = classProxyGenerator;
        }

        public Proxy Generate(LibraryDescription libraryDescription)
        {
            var toProxyClasses = new Queue<ClassDescription>();
            var proxiedClasses = new HashSet<string>();
            var namespaceUnits = new Dictionary<string, NamespaceUnit>();

            toProxyClasses.EnqueueRange(libraryDescription.Classes);

            while (toProxyClasses.Count != 0)
            {
                var classDescription = toProxyClasses.Dequeue();
                var toProxyDependencies = classDescription.Dependencies
                    .Where(el => el.PackageName != classDescription.PackageName)
                    .Where(el => !proxiedClasses.Contains(el.FullName));

                toProxyClasses.EnqueueRange(toProxyDependencies);
                proxiedClasses.Add(classDescription.FullName);

                var classProxy = classProxyGenerator.Generate(classDescription);

                var nestedClassProxies = classDescription.NestedClasses
                    .Select(el => classProxyGenerator.Generate(el))
                    .ToArray();

                classProxy = classProxy.AddMembers(nestedClassProxies);
                AddClassToNamespaceUnits(classProxy, classDescription, namespaceUnits);
            }

            return new Proxy(CollectCompilationUnitsFrom(namespaceUnits));
        }

        private IEnumerable<CompilationUnitSyntax> CollectCompilationUnitsFrom(
            Dictionary<string, NamespaceUnit> namespaceUnits)
        {
            foreach (var namespaceUnit in namespaceUnits.Values)
            {
                var namespaceDeclaration = namespaceUnit.NamespaceDeclaration;
                var usings = SyntaxFactory.List(namespaceUnit.Usings);

                var compilationUnit = SyntaxFactory.CompilationUnit()
                    .AddMembers(namespaceDeclaration)
                    .WithUsings(usings);

                yield return compilationUnit;
            }
        }

        private void AddClassToNamespaceUnits(
            MemberDeclarationSyntax classProxy,
            ClassDescription classDescription,
            IDictionary<string, NamespaceUnit> namespaceUnits)
        {
            var packageName = classDescription.PackageName;
            if (!namespaceUnits.ContainsKey(packageName))
                namespaceUnits[packageName] = new NamespaceUnit(packageName);

            var newDeclaration = namespaceUnits[packageName]
                .NamespaceDeclaration
                .AddMembers(classProxy);

            namespaceUnits[packageName].NamespaceDeclaration = newDeclaration;

            var dependenciesUsings = classDescription.Dependencies
                .Select(el => el.PackageName)
                .Select(el => SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(el)));

            namespaceUnits[packageName].Usings.AddRange(dependenciesUsings);
        }
    }
}
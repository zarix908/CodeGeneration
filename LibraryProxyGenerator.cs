using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class LibraryProxyGenerator : IGenerator<Proxy, LibraryDescription>
    {
        private readonly IGenerator<ClassDeclarationSyntax, ClassDescription> classProxyGenerator;

        public LibraryProxyGenerator(IGenerator<ClassDeclarationSyntax, ClassDescription> classProxyGenerator)
        {
            this.classProxyGenerator = classProxyGenerator;
        }

        public Proxy Generate(LibraryDescription libraryDescription)
        {
            var toProxyClasses = new Queue<ClassDescription>();
            var proxyClassesNames = new HashSet<string>();
            var namespaceUnits = new Dictionary<string, NamespaceUnit>();

            toProxyClasses.EnqueueRange(libraryDescription.Classes);

            while (toProxyClasses.Count != 0)
            {
                var classDescription = toProxyClasses.Dequeue();
                var toProxyDependencies = classDescription.DependenciesDescriptions
                    .Where(el => !proxyClassesNames.Contains(el.PackageName + el.Name));

                toProxyClasses.EnqueueRange(toProxyDependencies);
                proxyClassesNames.Add(classDescription.PackageName + classDescription.Name);

                var classProxy = classProxyGenerator.Generate(classDescription);

                var nestedClassProxies = classDescription.NestedClassesDescriptions
                    .Select(el => classProxyGenerator.Generate(el))
                    .ToArray();

                classProxy = classProxy.AddMembers(nestedClassProxies);
                AddClassToNamespaceUnits(classProxy, classDescription, namespaceUnits);
            }

            return new Proxy(namespaceUnits.Values);
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

            var dependenciesUsings = classDescription.DependenciesDescriptions
                .Select(el => el.PackageName)
                .Where(el => el != classDescription.PackageName)
                .Select(el => SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(el)));

            namespaceUnits[packageName].Usings.AddRange(dependenciesUsings);
        }
    }
}
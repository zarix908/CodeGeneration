using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenerateCode
{
    public class ProxyGenerator
    {
        private ProxyFiller proxyFiller;

        public string Generate(ProxyFiller proxyFiller)
        {
            this.proxyFiller = proxyFiller;

            var namespaceDeclaration = GenerateNamespace("SimpleCodeGeneration", new List<string> { "System" });

            var classDeclaration = GenerateClass("Proxy", new List<string>());
            var methodDeclaration = GenerateMethod("ConcatInt", "string", new List<string>{"public"}, 
                new List<Tuple<string, string>>
                {
                    Tuple.Create("a", "int"),
                    Tuple.Create("b", "int")
                });

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            return namespaceDeclaration
                .NormalizeWhitespace()
                .ToFullString();
        }

        private MethodDeclarationSyntax GenerateMethod(string name, string returnType, List<string> modifiers,
            List<Tuple<string, string>> parameters)
        {
            var parametersList = new ParameterSyntax[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameterDescribe = parameters.ElementAt(i);
                var parameterName = parameterDescribe.Item1;
                var parameterType = parameterDescribe.Item2;

                parametersList[i] = SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameterName))
                    .WithType(SyntaxFactory.ParseTypeName(parameterType));
            }

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnType), name)
                .AddParameterListParameters(parametersList)
                .WithBody(proxyFiller.GetMethodBody(name, parameters));

            foreach (var modifier in modifiers)
                methodDeclaration = methodDeclaration.AddModifiers(SyntaxFactory.ParseToken(modifier));

            return methodDeclaration;
        }

        private ClassDeclarationSyntax GenerateClass(string name, List<String> modifiers)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(name);

            foreach (var modifier in modifiers)
                classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.ParseToken(modifier));

            return classDeclaration;
        }

        private NamespaceDeclarationSyntax GenerateNamespace(string name, List<string> usings)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name)).NormalizeWhitespace();

            foreach (var @using in usings)
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(@using)));

            return @namespace;
        }
    }
}

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
        private ClassDescription classDescription;

        public ProxyGenerator(ProxyFiller proxyFiller, ClassDescription description)
        {
            this.proxyFiller = proxyFiller;
            classDescription = description;
        }

        public string Generate()
        {
            var namespaceDeclaration = GenerateNamespace("SimpleCodeGeneration", new List<string> { "System" });

            var classDeclaration = GenerateClass(classDescription.TypeName, classDescription.StatOrDyn);
            var methods = GenerateMethods();
            classDeclaration = classDeclaration.AddMembers(methods);
            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            return namespaceDeclaration
                .NormalizeWhitespace()
                .ToFullString();
        }

        private MethodDeclarationSyntax[] GenerateMethods()
        {
            var methodsCount = classDescription.Methods.Count;
            var methodDeclarationSyntaxes = new MethodDeclarationSyntax[methodsCount];

            for (int i = 0; i < methodsCount; i++)
            {
                var methodDescription = classDescription.Methods[i];
                var parameters = GenerateParameters(methodDescription);
                methodDeclarationSyntaxes[i] = GenerateMethod(methodDescription.MethodName, methodDescription.OutputType, parameters);
            }

            return methodDeclarationSyntaxes;
        }

        private List<Tuple<string, string>> GenerateParameters(MethodDescription methodDescription)
        {
            var parameters = new List<Tuple<String, String>>();

            var parameterNamePrefix = "arg_";
            for (int i = 0; i < methodDescription.InputTypes.Length; i++)
            {
                var parameterName = parameterNamePrefix + i;
                var parameterType = methodDescription.InputTypes[i];
                var parameter = Tuple.Create(parameterName, parameterType);
                parameters.Add(parameter);
            }

            return parameters;
        }

        private MethodDeclarationSyntax GenerateMethod(string name, string returnType,
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
                .WithBody(proxyFiller.GetMethodBody(name, parameters))
                .AddModifiers(SyntaxFactory.ParseToken("public"));

            return methodDeclaration;
        }

        private ClassDeclarationSyntax GenerateClass(string name, StaticOrDynamic staticOrDynamic)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(name);
            if (staticOrDynamic == StaticOrDynamic.Static)
                classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.ParseToken("static"));
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

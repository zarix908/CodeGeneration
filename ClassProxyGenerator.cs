using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class ClassProxyGenerator : IGenerator<ClassDeclarationSyntax, ClassDescription>
    {
        private readonly MethodBodyGetter methodBodyGetter;

        public ClassProxyGenerator(MethodBodyGetter methodBodyGetter) => this.methodBodyGetter = methodBodyGetter;

        public ClassDeclarationSyntax Generate(ClassDescription classDescription)
        {
            var modifiers = classDescription.ModifiersDescriptions
                .Select(Utils.ModifierDescriptionToSyntaxKind)
                .Select(SyntaxFactory.Token);

            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.Name)
                .AddModifiers(modifiers.ToArray());

            var publicFields = GenerateFields(classDescription.FieldsDescriptions
                .Where(el => el.ModifiersDescriptions.Contains(ModifierDescription.PUBLIC)));
            var publicMethods = GenerateMethods(classDescription.MethodsDescriptions
                .Where(el => el.ModifiersDescriptions.Contains(ModifierDescription.PUBLIC)));

            classDeclaration = classDeclaration
                .AddMembers(publicFields.ToArray())
                .AddMembers(publicMethods.ToArray());

            return classDeclaration;
        }

        private IEnumerable<FieldDeclarationSyntax> GenerateFields(IEnumerable<FieldDescription> fieldDescriptions)
        {
            foreach (var fieldDescription in fieldDescriptions)
            { 
                var fieldType = SyntaxFactory.ParseTypeName(fieldDescription.Type);
                var fieldName = SyntaxFactory.ParseToken(fieldDescription.Name);

                var variableDeclaration = SyntaxFactory.VariableDeclaration(fieldType)
                    .AddVariables(SyntaxFactory.VariableDeclarator(fieldName));
                var field = SyntaxFactory.FieldDeclaration(variableDeclaration);

                var modifiers = fieldDescription.ModifiersDescriptions
                    .Select(Utils.ModifierDescriptionToSyntaxKind)
                    .Select(SyntaxFactory.Token)
                    .ToArray();

                yield return field.AddModifiers(modifiers);
            }
        }

        private IEnumerable<MethodDeclarationSyntax> GenerateMethods(IEnumerable<MethodDescription> methodDescriptions)
        {
            foreach (var methodDescription in methodDescriptions)
            {
                var modifiers = methodDescription.ModifiersDescriptions
                    .Select(Utils.ModifierDescriptionToSyntaxKind)
                    .Select(SyntaxFactory.Token);

                var returnType = SyntaxFactory.ParseTypeName(methodDescription.ReturnType);

                if (methodDescription.ReturnType == "void")
                    returnType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));

                var methodDeclaration = SyntaxFactory
                    .MethodDeclaration(returnType,
                        methodDescription.Name)
                    .AddModifiers(modifiers.ToArray());

                var parameters = methodDescription.ParametersDescription
                    .Select(el => new
                        {
                            Name = SyntaxFactory.Identifier(el.Name),
                            Type = SyntaxFactory.ParseTypeName(el.Type)
                        })
                    .Select(el => SyntaxFactory.Parameter(el.Name).WithType(el.Type));

                methodDeclaration = methodDeclaration
                    .AddParameterListParameters(parameters.ToArray());

                yield return methodDeclaration.WithBody(methodBodyGetter.GetBodyFor(methodDeclaration));
            }
        }
    }
}

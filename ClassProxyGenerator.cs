using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator
{
    public class ClassProxyGenerator
    {
        public ClassDeclarationSyntax Generate(ClassDescription classDescription)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.Name);
            foreach (var modifierDescription in classDescription.Modifiers)
            {
                var syntaxKind = Utils.ModifierToSyntaxKind(modifierDescription);
                var modifier = SyntaxFactory.Token(syntaxKind);
                classDeclaration = classDeclaration.AddModifiers(modifier);
            }

            var publicFields = GenerateFields(classDescription.Fields
                .Where(el => el.Modifiers.Contains(Modifier.Public)));
            var publicMethods = GenerateMethods(classDescription.Methods
                .Where(el => el.Modifiers.Contains(Modifier.Public)));

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
                var field = SyntaxFactory.FieldDeclaration(variableDeclaration)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

                yield return field;
            }
        }

        private IEnumerable<MethodDeclarationSyntax> GenerateMethods(IEnumerable<MethodDescription> methodDescriptions)
        {
            foreach (var methodDescription in methodDescriptions)
            {
                var returnType = SyntaxFactory.ParseTypeName(methodDescription.ReturnType);
                var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, methodDescription.Name);

                foreach (var modifierDescription in methodDescription.Modifiers)
                {
                    var syntaxKind = Utils.ModifierToSyntaxKind(modifierDescription);
                    var modifier = SyntaxFactory.Token(syntaxKind);
                    methodDeclaration = methodDeclaration.AddModifiers(modifier);
                }

                foreach (var parameterDescription in methodDescription.ParametersDescription)
                {
                    var name = SyntaxFactory.Identifier(parameterDescription.Name);
                    var type = SyntaxFactory.ParseTypeName(parameterDescription.Type);
                    
                    var parameter = SyntaxFactory.Parameter(name)
                        .WithType(type);

                    methodDeclaration = methodDeclaration.AddParameterListParameters(parameter);
                }

                yield return methodDeclaration.WithBody(
                    SyntaxFactory.Block());
            }
        }
    }
}

using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace CodeGenerator
{
    [TestFixture]
    class ClassProxyGenerator_should
    {
        [Test]
        public void SimpleTest()
        {
            Assert.AreEqual(
@"public class MyClass
{
    public AField A;
    public static MyType MyMethod(String[] args)
    {
    }
}", TestBase(GenerateSimpleDescription()));
        }

        public string TestBase(ClassDescription classDescription)
        {
            var classProxyGenerator = new ClassProxyGenerator(new MethodBodyGetter());
            return classProxyGenerator.Generate(classDescription).NormalizeWhitespace().ToString();
        }

        private ClassDescription GenerateSimpleDescription()
        {
            var methodDescription = new MethodDescription(
                "MyMethod",
                new List<ModifierDescription> { ModifierDescription.PUBLIC, ModifierDescription.STATIC },
                new List<ParametrDescription> { new ParametrDescription("args", "String[]") },
                "MyType");

            var classStringDescription = new ClassDescription(
                "String",
                "java.lang",
                new List<ModifierDescription> { ModifierDescription.PUBLIC },
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<ClassDescription>(),
                new List<ClassDescription>(),
                isNested: false);

            var classMyTypeDescription = new ClassDescription(
                "MyType",
                "hello",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<ClassDescription>(),
                new List<ClassDescription>(),
                isNested: false);

            var nestedClassDescription = new ClassDescription(
                "Nested",
                null,
                new List<ModifierDescription> { ModifierDescription.PUBLIC },
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<ClassDescription>(),
                new List<ClassDescription>(),
                isNested: true);

            var classDescription = new ClassDescription(
                "MyClass",
                "hello",
                new List<ModifierDescription> { ModifierDescription.PUBLIC },
                new List<FieldDescription> { new FieldDescription("A", "AField", new List<ModifierDescription>{ModifierDescription.PUBLIC})},
                new List<MethodDescription> { methodDescription },
                new List<ClassDescription> { classStringDescription, classMyTypeDescription },
                new List<ClassDescription>() { nestedClassDescription },
                isNested: false);

            return classDescription;
        }
    }
}

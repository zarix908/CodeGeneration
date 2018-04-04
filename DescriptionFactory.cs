using System.Collections.Generic;
using DSL;

namespace CodeGenerator
{
    public class DescriptionFactory
    {
        public LibraryDescription Generate()
        {
            var methodDescription = new MethodDescription(
                "MyMethod",
                new List<Modifier> { Modifier.Public, Modifier.Static },
                new List<ParametrDescription>{new ParametrDescription("args", "String")}, 
                "void");

            var classStringDescription = new ClassDescription(
                "java.lang.String",
                new List<Modifier>{Modifier.Public},
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<ClassDescription>(),
                new List<ClassDescription>(),
                isNested: false);

            var classDescription = new ClassDescription(
                "hello.MyClass",
                new List<Modifier> {Modifier.Public},
                new List<FieldDescription>(),
                new List<MethodDescription>{methodDescription},
                new List<ClassDescription>{classStringDescription},
                new List<ClassDescription>(),
                isNested: false);

            return new LibraryDescription(new List<ClassDescription> { classDescription });
        }
    }
}

﻿using System.Collections.Generic;
using DSL;

namespace CodeGenerator
{
    public class DescriptionFactory
    {
        public LibraryDescription Generate()
        {
            var methodDescription = new MethodDescription(
                "MyMethod",
                new List<ModifierDescription> { ModifierDescription.PUBLIC, ModifierDescription.STATIC },
                new List<ParametrDescription>{new ParametrDescription("args", "String[]")}, 
                "void");

            var classStringDescription = new ClassDescription(
                "String",
                "java.lang",
                new List<ModifierDescription>{ModifierDescription.PUBLIC},
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
                new List<ModifierDescription> {ModifierDescription.PUBLIC},
                new List<FieldDescription>(),
                new List<MethodDescription>{methodDescription},
                new List<ClassDescription>{classStringDescription, classMyTypeDescription},
                new List<ClassDescription>() {nestedClassDescription},
                isNested: false);

            return new LibraryDescription("MyLibrary", new List<ClassDescription> { classDescription });
        }
    }
}

using System.Collections.Generic;

namespace GenerateCode
{
    public class ClassDescription : ITypeDescription
    {
        public StaticOrDynamic StatOrDyn;

        public ClassDescription(string name)
        {
            TypeName = name;
            Constructors = new List<ConstructorDescription>();
            Methods = new List<MethodDescription>();
            InnerClasses = new List<ClassDescription>();
        }

        public List<ConstructorDescription> Constructors;
        public List<MethodDescription> Methods;

        public List<ClassDescription> InnerClasses;
    }
}
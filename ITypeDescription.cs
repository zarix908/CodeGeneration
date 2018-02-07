using System;
using System.Collections.Generic;


namespace GenerateCode
{
    public enum StaticOrDynamic { Static, Dynamic };

    public class ITypeDescription
    {
        public static Dictionary<string, ITypeDescription> allTypes;
        public Dictionary<string, ITypeDescription> AllTypes;
        public Type RealType;
        public string TypeName;

        static ITypeDescription()
        {
            allTypes = new Dictionary<string, ITypeDescription>();
            new IntDescription();
            new DoubleDescription();
            new StringDescription();
        }

        public ITypeDescription()
        {
            AllTypes = allTypes;
        }
    }

    public class IntDescription : ITypeDescription
    {
        public IntDescription()
        {
            TypeName = "int";
            allTypes[TypeName] = this;
            RealType = typeof(int);
        }
    }

    public class DoubleDescription : ITypeDescription
    {
        public DoubleDescription()
        {
            TypeName = "double";
            allTypes[TypeName] = this;
            RealType = typeof(double);
        }
    }

    public class StringDescription : ITypeDescription
    {
        public StringDescription()
        {
            TypeName = "string";
            allTypes[TypeName] = this;
            RealType = typeof(string);
        }
    }
}
namespace GenerateCode
{
    public class ConstructorDescription
    {
        public string MethodName = "Constructor";
        public StaticOrDynamic StaticOrDynamic = StaticOrDynamic.Dynamic;

        public string[] InputTypes;
    }

    public class MethodDescription
    {
        public MethodDescription(string name)
        {
            MethodName = name;
        }

        public string MethodName;
        public StaticOrDynamic StaticOrDynamic;

        public string OutputType;
        public string[] InputTypes;
    }
}
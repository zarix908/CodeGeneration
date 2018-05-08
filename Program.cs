using System;

namespace CodeGenerator
{
    class Program
    {
        static void Main()
        {
            var description = new DescriptionFactory().Generate();
            var classGenerator = new ClassProxyGenerator(new MethodBodyGetter());
            var proxy = new LibraryProxyGenerator(classGenerator).Generate(description);
            new Compiller().Compile(proxy);
        }
    }
}
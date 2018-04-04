using System;

namespace CodeGenerator
{
    class Program
    {
        static void Main()
        {
            var description = new DescriptionFactory().Generate();
            var proxy = new LibraryProxyGenerator(new ClassProxyGenerator()).Generate(description);
            new Compiller().Compile(proxy);
            Console.ReadKey();
        }
    }
}
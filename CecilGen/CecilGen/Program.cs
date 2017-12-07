namespace CecilGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = new ProxyGenerator().Generate(new ProxyFiller());
            assembly.Write("SimpleCodeGeneration.dll");
        }
    }
}

using System;
using System.IO;

namespace GenerateCode
{
    class Program
    {
        static void Main()
        {
            var proxy = new ProxyGenerator().Generate(new ProxyFiller());
            var programFile = new StreamWriter("program.cs");
            programFile.Write(proxy);
            programFile.Close();
        }
    }
}
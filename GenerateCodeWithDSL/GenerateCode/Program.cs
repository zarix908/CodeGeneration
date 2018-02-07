using System;
using System.IO;

namespace GenerateCode
{
    class Program
    {
        static void Main()
        {
            var jsonClassDescription = new JsonClassDescriptionReader("source_file.txt").Read();
            var classDescription = new JsonClassDescriptionParser(jsonClassDescription).Parse();
            var proxy = new ProxyGenerator(new ProxyFiller(), classDescription).Generate();

            var programFile = new StreamWriter("program.cs");
            programFile.Write(proxy);
            programFile.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace CecilGen
{
    public class ProxyGenerator
    {
        private ProxyFiller proxyFiller;

        public AssemblyDefinition Generate(ProxyFiller proxyFiller)
        {
            this.proxyFiller = proxyFiller;

            var namespaceName = "SimpleCodeGeneration";

            var assembly = GenerateAssembly(namespaceName);
            var module = assembly.MainModule;

            var programType = GenerateClass(namespaceName, "Proxy", new List<TypeAttributes>
                {TypeAttributes.Class, TypeAttributes.Public}, module.Import(typeof(object)));

            module.Types.Add(programType);

            var mainMethod = GenerateMethod("ConcatInt", new List<MethodAttributes>
            {
                MethodAttributes.Public
            }, module.Import(typeof(void)), new List<ParameterDefinition>
            {
                new ParameterDefinition("a", new ParameterAttributes(), module.Import(typeof(int))),
                new ParameterDefinition("b", new ParameterAttributes(), module.Import(typeof(int)))
            });

            programType.Methods.Add(mainMethod);

            new ProxyFiller().SetMethodBody(mainMethod, module);

            return assembly;
        }

        private AssemblyDefinition GenerateAssembly(string name)
        {
            return AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(name, new Version()),
                name,
                ModuleKind.Dll);
        }

        private TypeDefinition GenerateClass(string @namespace,
            string name, List<TypeAttributes> attributes, TypeReference basType)
        {
            return new TypeDefinition(
                @namespace,
                name,
                attributes.Aggregate((a, b) => a | b),
                basType);
        }

        private MethodDefinition GenerateMethod(string name, List<MethodAttributes> attributes, 
            TypeReference returnType,
            List<ParameterDefinition> paeameters)
        {
            var method = new MethodDefinition(
                name,
                attributes.Aggregate((a, b) => a | b),
                returnType);

            foreach (var paeameter in paeameters)
                method.Parameters.Add(paeameter);

            return method;
        }
    }
}

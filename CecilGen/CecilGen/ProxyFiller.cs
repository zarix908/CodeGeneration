using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CecilGen
{
    public class ProxyFiller
    {
        public void SetMethodBody(MethodDefinition method, ModuleDefinition module)
        {
            var ilProcessor = method.Body.GetILProcessor();

            var convert = module.Import(
                typeof(Convert).GetMethod("ToString", new[] { typeof(int) }));

            ilProcessor.Emit(OpCodes.Ldarg_1);
            ilProcessor.Emit(OpCodes.Call, convert);;

            ilProcessor.Emit(OpCodes.Ldarg_2);
            ilProcessor.Emit(OpCodes.Call, convert);

            ilProcessor.Emit(OpCodes.Ldstr, "Helo, World!");

            var writeLine = module.Import(
                typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }));
            ilProcessor.Emit(OpCodes.Call, writeLine);

            ilProcessor.Emit(OpCodes.Pop);
            ilProcessor.Emit(OpCodes.Pop);
            ilProcessor.Emit(OpCodes.Ret);
        }
    }
}

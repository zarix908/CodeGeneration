using System.Collections.Generic;

namespace CodeGenerator
{
    public class Proxy
    {
        public IEnumerable<NamespaceUnit> NamespaceUnits { get; }

        public Proxy(IEnumerable<NamespaceUnit> namespaceUnits)
        {
            NamespaceUnits = namespaceUnits;
        }
    }
}

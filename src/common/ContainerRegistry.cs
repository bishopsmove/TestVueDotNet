using StructureMap;
using TestVueDotNet.Contract;

namespace TestVueDotNet.Common
{
    public class ContainerRegistry : Registry
    {
        public ContainerRegistry()
        {
            For<ICryptoService>().Use<CryptoHelper>();
        }
    }
}

using TestVueDotNet.Contract.Model;

namespace TestVueDotNet.Service.Model
{
    public class KeyValue : IKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
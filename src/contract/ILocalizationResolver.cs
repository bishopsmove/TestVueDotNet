using System.Collections.Generic;
using TestVueDotNet.Contract.Model;

namespace TestVueDotNet.Contract
{
    public interface ILocalizationResolver
    {
        IEnumerable<IKeyValue> ResolveSupportedCultures();
        object ResolveCulture(string cultureName);
    }
}

using System;
using System.Globalization;

namespace TestVueDotNet.Contract
{
    public interface IDomainContextResolver
    {
        IDomainContext Resolve(bool cache = true);
    }
}
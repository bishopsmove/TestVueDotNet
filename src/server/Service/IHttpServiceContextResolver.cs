using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using TestVueDotNet.Contract;

namespace TestVueDotNet.Server
{
    public interface IHttpServiceContextResolver : IDomainContextResolver
    {
        IUser Resolve(HttpContext context, bool cache = true);
    }
}
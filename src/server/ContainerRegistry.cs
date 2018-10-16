using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Configuration;
using StructureMap;
using TestVueDotNet.Contract;
using TestVueDotNet.Service;
using TestVueDotNet.Service.Localization;

using TestVueDotNet.Data;

namespace TestVueDotNet.Server
{
    internal class ContainerRegistry : Registry
    {
        public ContainerRegistry()
        {
            var targets = new Filters.ApiExceptionFilterTargets()
            {
                { typeof(TestVueDotNet.Service.ServiceException), PayloadMessageType.Failure}
            };

            For<IConfiguration>().Use(WebApp.Configuration).Singleton();
            
            For<DbContextBase>().Use<MsSqlContext>();
            
            For<HttpServiceContextFactory>();
            For<IHttpContextAccessor>().Use<HttpContextAccessor>().Transient();
            For<IHttpServiceContextResolver>().Use<HttpServiceContextResolver>();
            For<IDomainContextResolver>().Use<HttpServiceContextResolver>();
            For<IDeviceProfiler>().Use<HttpDeviceProfiler>();
            For<IVerificationProvider>().Use<HttpVerificationProvider>();
            For<ILocalizationResolver>().Add<LocalizationResolver>().Singleton();
            For<ILocalizationService>().Add<LocalizationService>();

            For<Filters.ApiResultFilter>();
            For<Filters.ApiExceptionFilterTargets>().Use(targets);
            For<Filters.ApiExceptionFilter>();
            For<Filters.IdentityMappingFilter>();

            For<CultureService>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TestVueDotNet.Contract;
using TestVueDotNet.Server.Model;
using TestVueDotNet.Server.Security;
using TestVueDotNet.Service;
using TestVueDotNet.Service.Security;

namespace TestVueDotNet.Server.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected ILocalizationService Localization { get; private set; }
        protected IDomainContextResolver Resolver;
        private ILocalizationDictionary dictionary;
        private IDomainContext domainContext;

        public ControllerBase(IDomainContextResolver resolver, ILocalizationService localization)
        {
            this.Localization = localization;
            this.Resolver = resolver;
        }

        protected ILocalizationDictionary Dictionary
        {
            get
            {
                if (this.dictionary == null)
                    this.dictionary = this.Localization.CreateDictionary(this.DomainContext);

                return this.dictionary;
            }
        }

        protected IDomainContext DomainContext
        {
            get
            {
                if (this.domainContext == null)
                    this.domainContext = this.Resolver.Resolve();

                return this.domainContext;
            }
        }

        protected void ThrowLocalizedServiceException(string key)
        {
            throw new ServiceException(this.Dictionary[key].Value);
        }
    }
}
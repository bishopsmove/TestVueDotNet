using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestVueDotNet.Contract;
using TestVueDotNet.Contract.Security;
using TestVueDotNet.Data;
using TestVueDotNet.Data.Model;

namespace TestVueDotNet.Service
{
    public class ExternalAuthenticationService : IExternalAuthenticationService
    {
        private readonly Config config;
        private readonly ICryptoService crypto;
        private readonly DbContextBase db;
        private readonly IDeviceProfiler deviceProfiler;
        private readonly IEnumerable<IExternalAuthenticationProvider> providers;

        public ExternalAuthenticationService(DbContextBase db, IOptions<Config> config, ICryptoService crypto, IDeviceProfiler deviceProfiler, IList<IExternalAuthenticationProvider> providers)
        {
            this.config = config.Value;
            this.crypto = crypto;
            this.db = db;
            this.deviceProfiler = deviceProfiler;
            this.providers = providers;
        }

        public Task<Nonce> CreateNonce()
        {
            return Task.Factory.StartNew(() =>
            {
                string salt = crypto.CreateSalt();
                string data = Guid.NewGuid().ToString();
                string hash = this.crypto.CreateKey(salt, data);

                return new Nonce(hash);
            });
        }

        public async Task<ClaimsIdentity> RedeemToken(IExternalLogin login)
        {
            var provider = this.providers.Single(o => o.ProviderId == login.ProviderId);

            var token = await provider.GetProfileDataFromProvider(login.AccessToken);

            if (token.aud != provider.ClientId)
                return null;

            User user = this.db.User.FirstOrDefault(o => o.Username == token.email);

            if (user == null)
            {
                user = await Task.Factory.StartNew(() => SignupUser(token.email));
                db.SaveChanges();
            }

            if (!user.Providers.Any(o => o.ProviderId == provider.ProviderId && o.ExternalId == token.sub))
            {
                db.UserProvider.Add(new UserProvider()
                {
                    ExternalId = token.sub,
                    ProviderId = provider.ProviderId,
                    User = user
                });

                db.SaveChanges();
            }

            string fingerprint = this.deviceProfiler.DeriveFingerprint(user);

            return user.ToClaimsIdentity(this.config.ClaimsNamespace, fingerprint);
        }

        public void RevokeToken(string providerId, string accessToken)
        {
            var provider = this.providers.Single(o => o.ProviderId == providerId);
            provider.RevokeToken(accessToken);
        }

        public async Task<bool> ValidateToken(string providerId, string accessToken)
        {
            bool validated = false;

            var provider = this.providers.Single(o => o.ProviderId == providerId);

            var token = await provider.GetProfileDataFromProvider(accessToken);

            if (token.aud == provider.ClientId)
                validated = true;

            return validated;
        }

        private User SignupUser(string email)
        {
            var user = new User()
            {
                CultureName = "en",    
                DisplayName = email.Split('@')[0],
                Enabled = true,
                TimeZoneId = Globalization.DefaultTimeZoneId,
                Username = email
            };

            db.User.Add(user);

            Role role = db.Role.FirstOrDefault(o => o.RoleId == RoleTypes.User);

            user.Roles.Add(new UserRole()
            {
                User = user,
                Role = role
            });

            return user;
        }
    }
}

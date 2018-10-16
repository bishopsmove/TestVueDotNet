using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using TestVueDotNet.Common;
using TestVueDotNet.Contract;
using TestVueDotNet.Data;

namespace TestVueDotNet.Server
{
    public partial class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AppConfig config = WebApp.Configuration.Get<TestVueDotNet.Server.AppConfig>();
            IConfigurationSection logging = WebApp.Configuration.GetSection("Logging");

            if (logging.GetSection("Debug").Exists())
                loggerFactory.AddDebug();

            if (logging.GetSection("Console").Exists())
                loggerFactory.AddConsole(logging.GetSection("Console"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
                {
                    HotModuleReplacement = true,
                    ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\ui")
                });
            }

            app.UseDefaultFiles();
            app.UseAuthentication();
            app.UseResponseCompression();

            string webRoot = new DirectoryInfo(config.Server.Webroot).FullName;

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(webRoot),
                OnPrepareResponse = (content) =>
                {
                    var cultureService = content.Context.RequestServices.GetRequiredService<CultureService>();
                    cultureService.EnsureCookie(content.Context);
                }
            });

            app.UseAntiforgeryMiddleware(config.Server.AntiForgery.ClientName);
            app.UseRequestLocalization();
            app.UseMvc();
            app.UseHistoryModeMiddleware(webRoot, config.Server.Areas);

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetService<DbContextBase>())
                {
                    ICryptoService crypto = app.ApplicationServices.GetRequiredService<ICryptoService>();
                    dbContext.Database.Migrate();
                    dbContext.EnsureSeedData(crypto);
                }
            }
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var config = WebApp.Configuration.Get<TestVueDotNet.Server.AppConfig>();

            services.AddOptions();
            services.Configure<AppConfig>(WebApp.Configuration); // root web configuration
            services.Configure<TestVueDotNet.Service.Config>(WebApp.Configuration.GetSection("service")); // services configuration
            services.Configure<TestVueDotNet.Service.TokenProviderConfig>(WebApp.Configuration.GetSection("service:tokenProvider")); // token provider configuration
            services.Configure<TestVueDotNet.Data.Config>(WebApp.Configuration.GetSection("data")); // configuration
            services.Configure<TestVueDotNet.Server.Config>(WebApp.Configuration.GetSection("server"));

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };
            });

            services.AddMemoryCache();
            services.AddDetection();
            services.ConfigureAuthentication(config.Service.TokenProvider, new string[] { "admin" });
            services.ConfigureMvc(config.Server.AntiForgery);
            
            services.AddDbContext<MsSqlContext>(options =>
            {
                string assemblyName = typeof(TestVueDotNet.Data.Config).GetAssemblyName();
                options.UseLazyLoadingProxies()
                    .UseSqlServer(config.Data.ConnectionString, s => s.MigrationsAssembly(assemblyName));
            });

            var container = new Container(c =>
            {
                var registry = new Registry();

                registry.IncludeRegistry<TestVueDotNet.Common.ContainerRegistry>();
                registry.IncludeRegistry<TestVueDotNet.Data.ContainerRegistry>();
                registry.IncludeRegistry<TestVueDotNet.Service.ContainerRegistry>();
                registry.IncludeRegistry<TestVueDotNet.Server.ContainerRegistry>();

                c.AddRegistry(registry);
                c.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
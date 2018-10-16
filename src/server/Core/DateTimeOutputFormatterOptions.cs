
using System;
using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TestVueDotNet.Contract;
using TestVueDotNet.Server.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using TestVueDotNet.Server.Formatters;

namespace TestVueDotNet.Server.Formatters
{
    public class DateTimeOutputFormatterOptions : IConfigureOptions<MvcOptions>
    {
        private readonly ArrayPool<char> charPool;

        public DateTimeOutputFormatterOptions(ArrayPool<char> charPool)
        {
            this.charPool = charPool;
        }

        public void Configure(MvcOptions options)
        {
            options.OutputFormatters.RemoveType<JsonOutputFormatter>();

            DateTimeOutputFormatterJson formatter = CreateCustomFormatter();

            options.OutputFormatters.Add(formatter);
        }

        private DateTimeOutputFormatterJson CreateCustomFormatter()
        {
            var serializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();

            return new DateTimeOutputFormatterJson(serializerSettings, charPool);
        }
    }
}
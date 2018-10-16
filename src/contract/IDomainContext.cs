using System;
using System.Globalization;

namespace TestVueDotNet.Contract
{
    public interface IDomainContext
    {
        CultureInfo Culture { get; set; }
        TimeZoneInfo SourceTimeZone { get; set; }
        IUser User { get; set; }
    }
}
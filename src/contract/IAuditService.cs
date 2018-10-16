using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestVueDotNet.Contract
{
    public interface IAuditService
    {
        void Record<T>(T data) where T : IAuditEventData;
        void Record<T>(IDomainContext domainContext, T data) where T : IAuditEventData;
    }
}

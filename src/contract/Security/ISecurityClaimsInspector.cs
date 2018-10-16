
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestVueDotNet.Contract.Security
{
    public interface ISecurityClaimsInspector
    {
        bool Satisifies(ClaimsPrincipal principal, ClaimRequirementType requirementType, string claimType, params object[] values);
    }
}
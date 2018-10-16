using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestVueDotNet.Contract
{
    public interface IDeviceProfiler
    {
        string DeriveFingerprint(IUser user);
    }
}
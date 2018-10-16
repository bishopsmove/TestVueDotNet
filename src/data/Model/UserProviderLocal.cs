using System;
using System.Collections.Generic;

namespace TestVueDotNet.Data.Model
{
    public partial class UserProviderLocal : UserProvider
    {
        public UserProviderLocal()
        {
            
        }

        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
}

using System;
using System.Linq;
using TestVueDotNet.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace TestVueDotNet.Server
{
   public static partial class Extensions
    {
        internal static string HttpContextCurrentUserKey = "CurrentUser";

        public static IUser ApplicationUser(this ControllerBase controller)
        {
            var user = controller.HttpContext.Items[HttpContextCurrentUserKey];

            return user == null ? null : (IUser)user;
        }
    }
}
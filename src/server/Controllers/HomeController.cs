using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestVueDotNet.Server.Model;

namespace TestVueDotNet.Server.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            string virtualPath = "index.html";
            string contentType = "text/html";

            return File(virtualPath, contentType);
        }

        public ActionResult Admin()
        {
            string virtualPath = "admin.html";
            string contentType = "text/html";

            return File(virtualPath, contentType);
        }
    }
}

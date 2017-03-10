using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;

namespace SpectoLogic.Azure.CDN.Sample.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IMediaUrlProvider cdnmedia)
        {
            string tokenCDNUrl = cdnmedia.Url("/images/cat.png", "myPolicy");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

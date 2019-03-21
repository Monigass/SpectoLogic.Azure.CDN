using Microsoft.AspNetCore.Mvc;

namespace VerizonDigital.CDN.TokenProvider.Sample.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICDNTokenProvider tokenProvider)
        {
            var tokenCDNUrl = tokenProvider.NewToken("default");
            var url = $"myurl?{tokenCDNUrl}";
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

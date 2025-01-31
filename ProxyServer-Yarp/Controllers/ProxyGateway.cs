using Microsoft.AspNetCore.Mvc;

namespace ProxyServer_Yarp.Controllers
{
    public class ProxyGateway : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

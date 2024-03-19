using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

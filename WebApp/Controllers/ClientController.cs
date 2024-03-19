using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Home()
        {
            
            return View();
        }
    }
}

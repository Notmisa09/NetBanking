using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Interfaces.Services;

namespace WebApp.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _adminService.GetAllAsync());
        }
    }
}

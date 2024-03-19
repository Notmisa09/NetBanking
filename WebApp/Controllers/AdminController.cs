using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

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
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }
        public async Task<IActionResult> DashBoard()
        {
            return View(await _adminService.GetDashboard());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;

namespace WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, 
                                IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
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

        //EDIT USER
        public async Task<IActionResult> Edit(string Id)
        {
            return View("Register", await _userService.GetByIdAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            };
            ServiceResult response = await _userService.UpdateAsync(vm);
            if (response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }

        //REMOVE USERS
        public async Task<IActionResult> Remove(string Id)
        {
            return View("Remove", await _userService.GetByIdAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTrue(string Id)
        {
            await _userService.Remove(Id);
            return View("Index");
        }
    }
}

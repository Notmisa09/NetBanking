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

        //INDEX
        public async Task<IActionResult> Index()
        {
            return View(await _adminService.GetAllAsync());
        }

        //DASHBORAD
        public async Task<IActionResult> DashBoard()
        {
            return View(await _adminService.GetDashboard());
        }

        //REGISTER USER
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        //LOGOUT
        public async Task<IActionResult> LogOut()
        {
            await _userService.SingOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //CHANGE USER STATUS
        public async Task<IActionResult> ChangeStatus(ActiveUserViewModel vm, string IdUser)
        {
            await _adminService.ChangeAccountStatus(vm);
            return RedirectToRoute(new { controller = "User", action = "Index" });
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
            return RedirectToAction("Index");
        }
    }
}

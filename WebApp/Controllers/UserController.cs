using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Dtos.Error;
using System.Diagnostics;
using WebApp.Models;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //INDEX

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        //CONFIRM EMAIL
        public async Task<IActionResult> ConfirmEmailAsync(string UserId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(UserId, token);
            return View("ConfirmEmail", response);
        }

        //LOGOUT

        public async Task<IActionResult> LogOut()
        {
            await _userService.SingOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //REGISTER
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ServiceResult response = await _userService.RegisterAsync(vm, origin, RolesEnum.Client.ToString());
            if (!response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            return RedirectToAction("Index");
        }


        // FORGOT PASSWORD
        public IActionResult ForgotPassword()
        {
            return View(new ForgorPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgorPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ServiceResult response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            return RedirectToRoute(new { controller="User", action="Index" });
        }


        //RESET PASSWORD
        public IActionResult ResetPassword(string Token)
        {
            return View(new ResetPasswordViewModel { Token = Token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View("ResetPassword", vm);
            }
            ServiceResult response = await _userService.ResetPasswordAsync(vm);
            if(response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View("ResetPassword", vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public async Task<IActionResult> EditUers(int Id)
        {
            return(await _userService.Ge)
        }
    }
}

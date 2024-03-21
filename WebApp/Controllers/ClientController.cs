using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Users;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpContextAccessor _contextAccessor;
        private readonly AuthenticationResponse user;
        private readonly ICreditCardService _creditCardService;
        public ClientController (HttpContextAccessor contextAccessor, 
                                  ICreditCardService creditCardService)
        {
            user = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _creditCardService = creditCardService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreditCardRequest(SaveUserViewModel vm)
        {
            await _creditCardService.CreateCardWithUser(vm);
            return RedirectToRoute(new {controller="Client", action="Index"});
        }
    }
}

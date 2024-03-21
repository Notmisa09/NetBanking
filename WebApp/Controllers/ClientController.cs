using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;


namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        public ClientController(IClientService clientService) 
        { 
            _clientService = clientService;
        }
        public async Task<IActionResult> Home()
        {
            var vm = await _clientService.GetAllProductsByClientAsync();
            return View(vm);
        }

        public async Task<IActionResult> Beneficiaries()
        {
            var vm = await _clientService.GetAllBeneficiariesByClientAsync();
            return View(vm);
        }

        public async Task<IActionResult> CreditCardRequest(SaveUserViewModel vm)
        {
            await _creditCardService.CreateCardWithUser(vm);
            return RedirectToRoute(new {controller="Client", action="Index"});
        }
    }
}

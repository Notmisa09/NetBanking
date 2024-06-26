﻿using Microsoft.AspNetCore.Mvc.Filters;
using NetBanking.Core.Application.Dtos.Account;
using WebApp.Controllers;
using NetBanking.Core.Application.Helpers;
namespace WebApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _validateUserSession;

        public LoginAuthorize(ValidateUserSession validateUserSession)
        {
            _validateUserSession = validateUserSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_validateUserSession.HasUser())
            {
                var controller = (UserController)context.Controller;
                context.Result = controller.RedirectToAction("index", "home");
            }
            else
            {
                await next();
            }
        }
    }
}

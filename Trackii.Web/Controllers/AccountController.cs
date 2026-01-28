using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Trackii.Application.Interfaces;
using Trackii.Web.ViewModels.Account;

namespace Trackii.Web.Controllers;

public sealed class AccountController : Controller
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username, ct);
            if (user is null)
            {
                model.ErrorMessage = "Usuario o contraseña incorrectos.";
                return View(model);
            }

            if (!user.Active)
            {
                model.ErrorMessage = "Usuario inactivo.";
                return View(model);
            }

            if (!string.Equals(user.PasswordHash, model.Password, StringComparison.Ordinal))
            {
                model.ErrorMessage = "Usuario o contraseña incorrectos.";
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            model.ErrorMessage = ex.Message;
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

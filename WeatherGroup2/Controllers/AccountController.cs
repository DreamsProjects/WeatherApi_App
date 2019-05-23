using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherGroup2.Identity;
using WeatherGroup2.ViewModels.AccountViewModels;

namespace WeatherGroup2.Controllers
{
     [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;


        public AccountController(AppIdentityDbContext context,
                                 UserManager<AppUser> usrMgr,
                                 SignInManager<AppUser> signInMgr,
                                 IUserValidator<AppUser> userValid,
                                 IPasswordValidator<AppUser> passValid,
                                 IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            signInManager = signInMgr;

            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }


        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,
                            lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Main", "Weather");
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email), "Incorrect password or username");
            }

            return View(model);
        }

        // POST: /Account/LogOut
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }


        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
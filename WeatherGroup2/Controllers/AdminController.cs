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
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager; 
        private SignInManager<AppUser> signInManager; 

        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;


        public AdminController(AppIdentityDbContext context,
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


        // GET: User/Create (Register)
        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult CreateUser()
        {
            return View();
        }


        // POST: User/Create (Register)
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                AppUser userInDb = await userManager.FindByNameAsync(userModel.Email);
                if (userInDb != null)
                {
                    ModelState.AddModelError("Username", "Det här användarnamnet är tagen. Välj ett annat.");
                    return View(userModel);
                }

                AppUser user = new AppUser()
                {
                    Email = userModel.Email,
                    UserName = userModel.Email,
                };

                IdentityResult result = await userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    // Assign the Role to the customer (Default: regular)
                  
                   // await userManager.AddToRoleAsync(user, MyAppRoles.regular);
                    
                    await signInManager.SignOutAsync(); 
                    await signInManager.SignInAsync(user, isPersistent: false); 

                    // For a normal user I need to redirect him to the Order Controller!
                    return RedirectToAction(nameof(WeatherController.Main), "Weather");
                    //  }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);
        }
    }
}

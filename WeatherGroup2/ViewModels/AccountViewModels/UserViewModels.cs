using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherGroup2.Identity;

namespace WeatherGroup2.ViewModels.AccountViewModels
{
    public class LoginModel
    {
        // NB - the UIHint attribute ensures that the input elements rendered 
        // by the tag helper in the view will have their type attributes set appropriately.

        // Username
        [EmailAddress]
        [Required(ErrorMessage = "E-mail(username) is required")]
        [Display(Name = "Username")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required")]
        // [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


    public class UserViewModel
    {
        public AppUser MyUser { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail(username) is required")]
        [Display(Name = "Username")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required")]
        // [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        // [Required(ErrorMessage = "The {0} is mandatory")]
        [Compare("Password", ErrorMessage = "Password don't match")]
        [Display(Name = "Verify password")]
        public string VerifyPassword { get; set; }
    }




}

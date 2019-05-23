using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WeatherGroup2.Models;

namespace WeatherGroup2.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Favorites = new HashSet<Favorite>();
        }

        // Navigation property
        public ICollection<Favorite> Favorites { get; set; }
    }
}

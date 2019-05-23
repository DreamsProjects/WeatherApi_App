using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherGroup2.Identity;

namespace WeatherGroup2.Models
{
    public class Favorite : IEquatable<Favorite>
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        // NAV
        public AppUser MyUser { get; set; }



        // Two favorites are Equals if CityId are equals
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null)) // || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Favorite item = (Favorite)obj;
            return (CityId == item.CityId);

        }

        public bool Equals(Favorite other)
        {
            if (other == null) return false;
            return (this.CityId.Equals(other.CityId));
        }

        public override int GetHashCode()
        {
            return CityId;
        }
    }
}

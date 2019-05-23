using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WeatherGroup2.Controllers;
using WeatherGroup2.Identity;
using WeatherGroup2.Models;


namespace WeatherGroup2.Repositories
{
    public class FavoritesRepository : IFavoriteRepository
    {

        private AppIdentityDbContext _context;


        public FavoritesRepository(AppIdentityDbContext context)
        {
            _context = context;
        }


        public IList<Favorite> Favorites
        {
            get => _context.Favorites.Include(c => c.MyUser).ToList();
            set { }
        }

        public async Task<int> Add(int cityId, AppUser loggedUser = null)
        {
            var IsNewItemInFavorites = Favorites.Any(c => c.CityId == cityId && c.MyUser.Id == loggedUser?.Id);

            if (!IsNewItemInFavorites)
            {
                var httpClient = new WeatherHTTPClientController();
                // Search details on city from API
                WeatherData cityInfo = httpClient.GetByCityForecast(cityId.ToString());

                _context.Favorites.Add(new Favorite
                {
                    CityId = cityId,
                    CityName = cityInfo.City.Name,
                    CountryName = cityInfo.City.Country,
                    MyUser = loggedUser
                });
                return 0;
            }
            // Error, item is already in favorites
            return -1;
        }


        public int Remove(int cityId, string loggedUserId = null)
        {
            var favoriteToBeRemoved = Favorites.FirstOrDefault(c => c.MyUser.Id == loggedUserId && c.CityId == cityId);

            if (favoriteToBeRemoved != null)
            {
                _context.Favorites.Remove(favoriteToBeRemoved);
                return 0;
            }
            // Error, item cannot be removed because is NOT in the list yet
            return -1;
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using WeatherGroup2.Identity;
using WeatherGroup2.Models;

namespace WeatherGroup2.Repositories
{
    public class MockFavoriteRepository : IFavoriteRepository
    {
        private IList<Favorite> temporaryFavoritesChanges;

        public IList<Favorite> Favorites { get; set; } = new List<Favorite>
        {
            new Favorite {Id = 1, CityId = 20, CityName = "Stockholm", CountryName = "Sweden"},
            new Favorite {Id = 2, CityId = 30, CityName = "Lund", CountryName = "Sweden"},
            new Favorite {Id = 3, CityId = 40, CityName = "Göteborg", CountryName = "Sweden"},
            new Favorite {Id = 4, CityId = 50, CityName = "NewYork", CountryName = "USA"}
        };


        public async Task<int> Add(int cityId, AppUser optional = null)
        {
            var IsNewItemInFavorites = Favorites.Any(c => c.CityId == cityId);
            if (!IsNewItemInFavorites)
            {
                temporaryFavoritesChanges = Favorites.ToList();

                temporaryFavoritesChanges.Add(new Favorite
                {
                    CityId = cityId
                });
                return 0;
            }

            // Error, item is already in favorites
            return -1;

        }

        public int Remove(int cityId, string loggedUserId = null)
        {
            var favoriteToBeRemoved = Favorites.FirstOrDefault(c => c.CityId == cityId);
            if (favoriteToBeRemoved != null)
            {
                temporaryFavoritesChanges = Favorites.ToList();

                temporaryFavoritesChanges.Remove(favoriteToBeRemoved);

                return 0;
            }

            // Error, item cannot be removed because is NOT in the list yet
            return -1;

        }

        public void Save()
        {
            Favorites = temporaryFavoritesChanges.ToList();

        }


    }
}

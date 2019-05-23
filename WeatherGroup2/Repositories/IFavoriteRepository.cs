using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherGroup2.Identity;
using WeatherGroup2.Models;

namespace WeatherGroup2.Repositories
{
    public interface IFavoriteRepository
    {
        IList<Favorite> Favorites { get; set; }

        Task<int> Add(int cityId, AppUser loggedUser = null);
        int Remove(int id, string loggedUserId = null);
        void Save();
    }
}

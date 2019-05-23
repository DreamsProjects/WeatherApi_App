using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherGroup2.Identity;
using WeatherGroup2.Models;
using WeatherGroup2.Repositories;
using WeatherGroup2.ViewModels;

namespace WeatherGroup2.Controllers
{
    //[Authorize]
    public class WeatherController : Controller
    {
        private AppIdentityDbContext _context;
        private IFavoriteRepository _favoriteRepository;

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public WeatherController(IFavoriteRepository favoriteRepo,
                                 AppIdentityDbContext context,
                                 UserManager<AppUser> usrMgr,
                                 SignInManager<AppUser> signInMgr)
        {
            userManager = usrMgr;
            signInManager = signInMgr;

            _context = context;
            _favoriteRepository = favoriteRepo;
            userManager = usrMgr;
            signInManager = signInMgr;
        }



        public async Task<PartialViewResult> ShowOverview(string searchQuery = "2673730") //Default startsida är stockholm
        {
            var httpClient = new WeatherHTTPClientController();
            var result = httpClient.GetByCityToday(searchQuery);

            var viewModel = new OverviewViewModel()
            {
                Day = result.Date,
                Date = result.Date.ToString("dd/MMM", new CultureInfo("en-US")),
                CityId = result.CityId,
                CityName = result.CityName,
                CurrentTemp = Convert.ToInt32(result.Temperature.CurrentTemperature),
                Weather = result.Weather[0].WeatherDescription,
                WeatherUrl = result.Weather[0].IconImageUrl
            };

            return PartialView("ShowOverviewPartial", viewModel);
        }


        public PartialViewResult ShowDetails(string cityId)
        {
            var httpClient = new WeatherHTTPClientController();
            var detailedResult = httpClient.GetByCityForecast(cityId);

            var viewModel = new DetailedViewModel()
            {
                CityName = detailedResult.City.Name,
                CityId = cityId
            };

            var testSplitDays = detailedResult.DetailedInformation.Select(x => x.Date.Day).Distinct().ToList();

            foreach (var value in testSplitDays)
            {
                var temperatures = detailedResult.DetailedInformation.Where(x => x.Date.Day == value).ToList();

                double maxForDay = 0;
                double minForDay = temperatures[0].Temperature.MinTemperature;


                foreach (var tempValue in temperatures)
                {
                    if (tempValue.Temperature.MinTemperature < minForDay)
                    {
                        minForDay = tempValue.Temperature.MinTemperature;
                    }

                    if (tempValue.Temperature.MaxTemperature > maxForDay)
                    {
                        maxForDay = tempValue.Temperature.MaxTemperature;
                    }
                }


                var item = detailedResult.DetailedInformation.FirstOrDefault(x => x.Date.Day == value && x.Date.Hour >= 14);

                if (item == null)
                {
                    item = detailedResult.DetailedInformation.FirstOrDefault(x => x.Date.Day == value);
                }

                var Info = new DetailedInformation();
                Info.Date = item.Date;
                Info.TemperatureMax = Convert.ToInt32(maxForDay);
                Info.TemperatureMin = Convert.ToInt32(minForDay);
                Info.Weather = item.Weather[0].WeatherDescription;
                Info.WeatherIconUrl = item.Weather[0].IconImageUrl;
                Info.Day = item.Date.ToString("dddd", new CultureInfo("en-US"));

                viewModel.DetailedInformation.Add(Info);
            }

            return PartialView("ShowDetailsPartial", viewModel);
        }


        [HttpGet]
        public PartialViewResult ShowFavorites()
        {
            string currentLoggedUserID = userManager.GetUserId(User);

            var model = _favoriteRepository.Favorites.Where(c => c.MyUser.Id == currentLoggedUserID);

            return PartialView("ShowFavoritePartial", model);
        }


        public async Task<PartialViewResult> SaveInFavorites(int favoriteId)
        {
            AppUser loggedUser = await userManager.GetUserAsync(User);

            var result = await _favoriteRepository.Add(favoriteId, loggedUser);

            if (result == -1)
            {
                // Error: place not in Storage
                // Message to User in view!  TO DO !
            }
            else
            {

                // Message to user in view! To Do !

                _favoriteRepository.Save();
            }

            var model = _favoriteRepository.Favorites.Where(c => c.MyUser.Id == loggedUser.Id);

            return PartialView("ShowFavoritePartial", model);
        }


        public PartialViewResult RemoveFromFavorites(int favoriteId)
        {
            string loggedUserId = userManager.GetUserId(User);

            var result = _favoriteRepository.Remove(favoriteId, loggedUserId);

            if (result == -1)
            {
                // Error: place not in Storage
                // Message to User in view!  TO DO !
            }
            else
            {
                // Message t user in view! To Do !
                _favoriteRepository.Save();
            }

            var model = _favoriteRepository.Favorites.Where(c => c.MyUser.Id == loggedUserId);

            return PartialView("ShowFavoritePartial", model);
        }


        public PartialViewResult ShowSearchResults(string searchQuery)
        {
            var httpClient = new WeatherHTTPClientController();
            var resultForecast = httpClient.GetSearchResultsGetByQuery(searchQuery);
            //return View(resultForecast);
            return PartialView("ShowSearchResultsPartial", resultForecast);
        }


        public IActionResult Main(string searchQuery = "2673730") //Default startsida är stockholm)
        {

            var httpClient = new WeatherHTTPClientController();
            var result = httpClient.GetByCityToday(searchQuery);

            var viewModel = new OverviewViewModel()
            {
                Day = result.Date,
                Date = result.Date.ToString("dd/MMM"),
                CityId = result.CityId,
                CityName = result.CityName,
                CurrentTemp = Convert.ToInt32(result.Temperature.CurrentTemperature),
                Weather = result.Weather[0].WeatherDescription,
                WeatherUrl = result.Weather[0].IconImageUrl
            };

            return View(viewModel);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherGroup2.Models;

namespace WeatherGroup2.Controllers
{
    public class WeatherHTTPClientController : Controller
    {
        public string apiKey = "fc4dbbf2a03c3f80f8366cbb3c15c421";
        public HttpClient client;

        public WeatherHTTPClientController()
        {
            client = new HttpClient() { BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/") };
        }

        public WeatherDetails2 GetByCityToday(string cityId)
        {
            var serializer = new DataContractJsonSerializer(typeof(WeatherDetails2));
            // var response = client.GetAsync($"weather?q={cityName}&appid={apiKey}").Result;
            var response = client.GetAsync($"weather?id={cityId}&appid={apiKey}").Result;
            var responseStream = response.Content.ReadAsStreamAsync().Result;

            var weatherResult = (WeatherDetails2)serializer.ReadObject(responseStream);

            return weatherResult;

        }

        public WeatherData GetByCityForecast(string cityId)
        {
            var serializer = new DataContractJsonSerializer(typeof(WeatherData));
            var response = client.GetAsync($"forecast?id={cityId}&appid={apiKey}").Result;
            var responseStream = response.Content.ReadAsStreamAsync().Result;


            var weatherResult = (WeatherData)serializer.ReadObject(responseStream);
            var testSplitDays = weatherResult.DetailedInformation.Select(x => x.Date.Day).Distinct().ToList();
            var returnListOfFirstResultOfDay = new List<WeatherDetails2>();
            foreach (var testSplitDay in testSplitDays)
            {
                var testGetSplitDays = weatherResult.DetailedInformation.FirstOrDefault(x => x.Date.Day == testSplitDay);
                returnListOfFirstResultOfDay.Add(testGetSplitDays);
            }

            return weatherResult;
        }

        public WeatherSearch GetSearchResultsGetByQuery(string query)
        {
            var serializer = new DataContractJsonSerializer(typeof(WeatherSearch));
            var response = client.GetAsync($"find?q={query}&type=like&appid={apiKey}").Result;
            var responseStream = response.Content.ReadAsStreamAsync().Result;

            var searchResult = (WeatherSearch)serializer.ReadObject(responseStream);

            //Istället för att bara få platsnamn och land så får man tillbaka ikoner/väder just nu... tog med det för tillfället 
            //eftersom det är så lätt att göra resultaten lite snyggare

            return searchResult;
            //Får lista av de olika matchningar, vi vill ha Platsnamn/Id till platser som vi sedan kan skicka in i GetByCity() metoden.
        }
    }
}
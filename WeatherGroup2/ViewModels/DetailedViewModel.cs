using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherGroup2.ViewModels
{
    public class DetailedViewModel
    {
        public string CityId { get; set; }

        public string CityName { get; set; }

        public List<DetailedInformation> DetailedInformation { get; set; }

        public DetailedViewModel()
        {
            DetailedInformation = new List<DetailedInformation>();
        }
    }

    public class DetailedInformation
    {
        public DateTime Date { get; set; }

        public string Weather { get; set; }

        public string WeatherIconUrl { get; set; }

        public int TemperatureMin { get; set; }

        public int TemperatureMax { get; set; }

        public string Day { get; set; }
    }
}

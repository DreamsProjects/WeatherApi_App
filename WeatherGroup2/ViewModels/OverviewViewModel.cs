using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherGroup2.ViewModels
{
    public class OverviewViewModel
    {
        public string CityId { get; set; }

        public string CityName { get; set; }

        public string Date { get; set; }

        public string Weather { get; set; }

        public string WeatherUrl { get; set; }

        public int CurrentTemp { get; set; }

        public DateTime Day { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Threading.Tasks;

namespace WeatherGroup2.Models
{
    [DataContract]
    public class WeatherSearch
    {
       [DataMember(Name = "list")]
        public List<Cities> CityList { get; set; }
    }

    [DataContract]
    public class Cities
    {
        [DataMember(Name = "id")]
        public int CityId { get; set; }
        [DataMember(Name = "name")]
        public string CityName { get; set; }
        [DataMember(Name = "main")]
        public Temperatures CityWeather { get; set; }
        [DataMember(Name = "sys")]
        public Country Country { get; set; }
        [DataMember(Name = "weather")]
        public Weather[] Weather { get; set; }
    }

    [DataContract]
    public class Country
    {
        [DataMember(Name = "country")]
        public string CountryName { get; set; }
    }
}

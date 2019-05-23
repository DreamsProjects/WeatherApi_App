using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WeatherGroup2.Models
{
    [DataContract]
    public class WeatherData
    {
        [DataMember(Name = "city")]
        public City City { get; set; }

        [DataMember(Name = "list")]
        public List<WeatherDetails2> DetailedInformation{ get; set; }
    }

    [DataContract]
    public class City
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}

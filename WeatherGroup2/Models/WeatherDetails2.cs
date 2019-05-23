using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WeatherGroup2.Models
{
    [DataContract]
    public class WeatherDetails2
    {
        [DataMember(Name = "id")]
        public string CityId { get; set; }

        [DataMember(Name = "name")]
        public string CityName { get; set; }

        [DataMember(Name = "main")]
        public Temperatures Temperature { get; set; }

        public DateTime Date { get; set; }

        [DataMember(Name = "weather")]
        public Weather[] Weather { get; set; }

        [DataMember(Name = "dt")]
        protected string UnixDateTime
        {
            get => "";
            set
            {
                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                Date = Date.AddSeconds(double.Parse(value)).ToLocalTime();
            }
        }
    }

    [DataContract]
    public class Weather
    {
        [DataMember(Name = "main")]
        public string QuickDescription { get; set; }

        [DataMember(Name = "description")]
        public string WeatherDescription { get; set; }

        public string IconImageUrl { get; set; }

        [DataMember(Name = "icon")]
        public string IncomingIconId
        {
            get => "";
            set => IconImageUrl = $"http://openweathermap.org/img/w/{value}.png";
        }

    }

    [DataContract]
    public class Temperatures
    {
        public double CurrentTemperature { get; set; }

        public double MaxTemperature { get; set; }

        public double MinTemperature { get; set; }

        [DataMember(Name = "temp")]
        protected double IncomingTemp
        {
            get => 0;
            set => CurrentTemperature = GetTemperatureInCelsius(value);
        }

        [DataMember(Name = "temp_max")]
        protected double IncomingMaxTemp
        {
            get => 0;
            set => MaxTemperature = GetTemperatureInCelsius(value);
        }

        [DataMember(Name = "temp_min")]
        protected double IncomingMinTemp
        {
            get => 0;
            set => MinTemperature = GetTemperatureInCelsius(value);
        }

        public double GetTemperatureInCelsius(double temperatureInKelvin)
        {
            return (Math.Ceiling(temperatureInKelvin) - 273);
        }
    }
}


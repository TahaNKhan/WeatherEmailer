using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherEmailer.Logic.Api.AccuWeatherService.Exceptions
{
    public class LocationNotFoundException: Exception
    {
        public string LocationName { get; private set; }
        
        public LocationNotFoundException(string locationName)
        {
            LocationName = locationName;
        }

        public override string ToString()
        {

            return $"Location not found: {LocationName}. Exception Details: " + base.ToString();
        }
    }
}

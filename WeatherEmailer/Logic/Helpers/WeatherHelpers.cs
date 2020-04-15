using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherEmailer.Logic.Helpers
{
    public static class WeatherHelpers
    {
        public static decimal ToCelsius(this decimal fahrenheit)
        {
            return Math.Round((fahrenheit - 32m) * (5.0m / 9.0m), 1);
        }
    }
}

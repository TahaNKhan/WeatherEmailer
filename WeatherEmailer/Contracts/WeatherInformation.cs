using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherEmailer.Contracts
{
    public class WeatherInformation
    {
        public string Headline { get; set; }
        public Temperature Temperature { get; set; }
    }
}

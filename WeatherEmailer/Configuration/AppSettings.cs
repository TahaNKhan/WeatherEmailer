using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherTextMessager.Configuration
{
    public class AppSettings
    {
        public bool SendEmails { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public AccuWeatherSettings AccuWeatherSettings { get; set; }
        public string EmailsCSV { get; set; }
        public IEnumerable<string> Emails => EmailsCSV.Split(',');
        public string LoggingLocation { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
    }

    public class AccuWeatherSettings
    {
        public string ApiKey { get; set; }
        public bool UseActualService { get; set; }
    }
}

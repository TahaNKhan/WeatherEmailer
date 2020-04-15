using System;
using System.Collections.Generic;

namespace WeatherEmailer.Logic.Api.AccuWeatherService.Models
{

    public class DailyForecastResponse
    {
        public Headline Headline { get; set; }
        public IEnumerable<DailyForecast> DailyForecasts { get; set; }
    }

    public class Headline
    {
        public DateTimeOffset EffectiveDate { get; set; }
        public decimal EffectiveEpochDate { get; set; }
        public decimal Severity { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal EndEpochDate { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class DailyForecast
    {
        public DateTimeOffset Date { get; set; }
        public decimal EpochDate { get; set; }
        public Temperature Temperature { get; set; }
        public Day Day { get; set; }
        public Night Night { get; set; }
        public string[] Sources { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class Temperature
    {
        public TemperatureUnit Minimum { get; set; }
        public TemperatureUnit Maximum { get; set; }
    }

    public class TemperatureUnit
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal UnitType { get; set; }
    }

    public class Day
    {
        public decimal Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
    }

    public class Night
    {
        public decimal Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public string Precipitationdecimalensity { get; set; }
    }

}

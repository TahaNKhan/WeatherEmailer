using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherEmailer.Logic.Api.AccuWeatherService.Models;
using WeatherEmailer.Configuration;
using WeatherEmailer.Logic.Api.Interfaces;
using WeatherEmailer.Contracts;

namespace WeatherEmailer.Logic.Api.AccuWeatherService
{
    public class AccuWeatherSerivce: IWeatherService
    {
        private readonly WeatherEmailer.Logging.ILogger _logger;
        private readonly WeatherEmailer.Configuration.AppSettings _appSettings;
        public AccuWeatherSerivce(Logging.ILogger logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        #region IWeatherService
        public async Task<WeatherInformation> GetWeatherInformationAsync(string city, string state, CancellationToken cancellationToken = default)
        {
            var locationName = $"{city} {state}";
            var citySearchResults = await SearchCity(locationName, cancellationToken);
            var bestMatch = citySearchResults.FirstOrDefault();
            if (bestMatch == null)
                throw new Exceptions.LocationNotFoundException(locationName);
            var weatherInfo = await GetWeatherData(bestMatch.Key, cancellationToken);
            var mappedContract = MapAccuWeatherDataToContract(weatherInfo);
            return mappedContract;
        }
        #endregion IWeatherService

        internal virtual async Task<IEnumerable<CitySearchResult>> SearchCity(string cityWithState, CancellationToken cancellationToken = default)
        {
            var apiKey = _appSettings.AccuWeatherSettings.ApiKey;
            var baseUrl = "http://dataservice.accuweather.com/locations/v1/cities/search";
            baseUrl += $"?q={HttpUtility.UrlEncode(cityWithState)}";
            baseUrl += $"&apikey={apiKey}";
            baseUrl += $"&language=en-us";

            _logger.Log($"Making a HTTP request to AccuWeather '{baseUrl.Replace(_appSettings.AccuWeatherSettings.ApiKey, "REDACTED")}'");

            using var webClient = new HttpClient();

            try
            {
                var result = await webClient.GetAsync(baseUrl, cancellationToken);
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    _logger.Log("Failed to search cities in accuweather, details:");
                    _logger.Log($"Status code: {(int)result.StatusCode}");
                    _logger.Log($"Content: {await result.Content.ReadAsStringAsync()}");
                    // End processing
                    throw new Exception();
                }
                var contentString = await result.Content.ReadAsStringAsync();
                _logger.Log("Sucessfully made a request to AccuWeather");
                _logger.Log($"Response: {contentString}");
                return JsonSerializer.Deserialize<IEnumerable<CitySearchResult>>(contentString);
            }
            catch (HttpRequestException)
            {
                _logger.Log("Failed to search cities in accuweather");
                throw;
            }

        }

        internal virtual async Task<DailyForecastResponse> GetWeatherData(string locationKey, CancellationToken cancellationToken = default)
        {
            var apiKey = _appSettings.AccuWeatherSettings.ApiKey;
            var baseUrl = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{locationKey}";
            baseUrl += $"?apikey={apiKey}";
            baseUrl += $"&language=en-us";

            _logger.Log($"Making a HTTP request to AccuWeather '{baseUrl.Replace(_appSettings.AccuWeatherSettings.ApiKey, "REDACTED")}'");

            using var webClient = new HttpClient();

            try
            {
                var result = await webClient.GetAsync(baseUrl, cancellationToken);
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    _logger.Log("Failed to get weather data, details:");
                    _logger.Log($"Status code: {(int)result.StatusCode}");
                    _logger.Log($"Content: {await result.Content.ReadAsStringAsync()}");
                    // End processing
                    throw new Exception();
                }
                var contentString = await result.Content.ReadAsStringAsync();
                _logger.Log("Sucessfully made a request to AccuWeather");
                _logger.Log($"Response: {contentString}");
                return JsonSerializer.Deserialize<DailyForecastResponse>(contentString);
            }
            catch (HttpRequestException)
            {
                _logger.Log("Failed to get weather data.");
                throw;
            }
        }

        #region Mappers

        internal virtual Contracts.WeatherInformation MapAccuWeatherDataToContract(DailyForecastResponse accuWeatherModel)
        {
            var forecast = accuWeatherModel.DailyForecasts.First();
            var headline = accuWeatherModel.Headline.Text;
            var high = MapAccuWeatherTempatureToContract(forecast.Temperature.Maximum);
            var low = MapAccuWeatherTempatureToContract(forecast.Temperature.Minimum);
            return new Contracts.WeatherInformation
            {
                Headline = headline,
                Temperature = new Contracts.Temperature
                {
                    High = high,
                    Low = low
                }
            };

        }

        internal virtual TemperatureValue MapAccuWeatherTempatureToContract(Models.TemperatureUnit accuWeatherTemperature)
        {
            return new TemperatureValue
            {
                Unit = MapAccuWeatherTemperatureUnitToContract(accuWeatherTemperature.Unit),
                Value = accuWeatherTemperature.Value
            };
        }

        internal virtual TemperatureValue.TemperatureUnit MapAccuWeatherTemperatureUnitToContract(string unit)
        {
            return unit switch
            {
                "F" => TemperatureValue.TemperatureUnit.Fahrenheit,
                "C" => TemperatureValue.TemperatureUnit.Celsius,
                _ => throw new Exception("Invalid temperature unit"),
            };
        }

        #endregion Mappers
    }
}

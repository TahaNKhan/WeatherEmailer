using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherTextMessager.Configuration;
using WeatherTextMessager.Logic.Api.AccuWeatherServiceModels;

namespace WeatherTextMessager.Logic.Api
{
    public interface IAccuWeatherSerivce 
    {
        Task<DailyForecastResponse> GetWeatherData(string locationKey, CancellationToken cancellationToken = default);
        Task<IEnumerable<CitySearchResult>> SearchCity(string cityWithState, CancellationToken cancellationToken = default);
    }
    public class AccuWeatherSerivce : IAccuWeatherSerivce
    {
        private Logging.ILogger _logger;
        private Configuration.AppSettings _appSettings;
        public AccuWeatherSerivce(Logging.ILogger logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task<IEnumerable<CitySearchResult>> SearchCity(string cityWithState, CancellationToken cancellationToken = default)
        {
            var apiKey = _appSettings.AccuWeatherApiKey;
            var baseUrl = "http://dataservice.accuweather.com/locations/v1/cities/search";
            baseUrl += $"?q={HttpUtility.UrlEncode(cityWithState)}";
            baseUrl += $"&apikey={apiKey}";
            baseUrl += $"&language=en-us";

            _logger.Log($"Making a HTTP request to AccuWeather '{baseUrl.Replace(_appSettings.AccuWeatherApiKey, "REDACTED")}'");

            using var webClient = new HttpClient();

            try
            {
                var result = await webClient.GetAsync(baseUrl, cancellationToken);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
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

        public async Task<DailyForecastResponse> GetWeatherData(string locationKey, CancellationToken cancellationToken = default)
        {
            var apiKey = _appSettings.AccuWeatherApiKey;
            var baseUrl = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{locationKey}";
            baseUrl += $"?apikey={apiKey}";
            baseUrl += $"&language=en-us";

            _logger.Log($"Making a HTTP request to AccuWeather '{baseUrl.Replace(_appSettings.AccuWeatherApiKey, "REDACTED")}'");

            using var webClient = new HttpClient();

            try
            {
                var result = await webClient.GetAsync(baseUrl, cancellationToken);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
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
            catch (HttpRequestException ex)
            {
                _logger.Log("Failed to get weather data, details:");
                _logger.Log(ex.ToString());
                return null;
            }
        }
    }
}

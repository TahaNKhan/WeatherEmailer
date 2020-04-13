using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherTextMessager.Configuration;
using WeatherTextMessager.Logging;
using WeatherTextMessager.Logic.Api;
using WeatherTextMessager.Logic.Api.AccuWeatherServiceModels;
using WeatherTextMessager.Logic.Helpers;

namespace WeatherTextMessager.Logic
{
    public interface ISendWeatherInformationTask
    {
        Task SendWeatherInformation(CancellationToken cancellationToken = default);
    }
    public class SendWeatherInformationTask: ISendWeatherInformationTask
    {
        private readonly ILogger _logger;
        private readonly IServiceProxyFactory _serviceProxyFactory;
        private readonly AppSettings _appSettings;
        public SendWeatherInformationTask(ILogger logger, IServiceProxyFactory serviceProxyFactory, AppSettings appSettings)
        {
            _logger = logger;
            _serviceProxyFactory = serviceProxyFactory;
            _appSettings = appSettings;
        }

        public async Task SendWeatherInformation(CancellationToken cancellationToken = default)
        {
            var cityWithState = $"{_appSettings.CurrentCity} {_appSettings.CurrentState}";

            var accuWeatherSerivce = _serviceProxyFactory.GetAccuWeatherSerivce();
            var locationKey = await ObtainLocationKey(accuWeatherSerivce, cityWithState, cancellationToken);

            var weatherResponse = await accuWeatherSerivce.GetWeatherData(locationKey, cancellationToken);

            var subject = $"Weather Update - {cityWithState}";
            var body = BuildEmailBody(weatherResponse);
            var gmailService = _serviceProxyFactory.GetGmailSerivce();
            await gmailService.SendEmailAsync(_appSettings.Emails, subject, body.ToString());
        }

        internal virtual string BuildEmailBody(DailyForecastResponse weatherResponse)
        {
            var dailyForecast = weatherResponse.DailyForecasts.First();
            var body = new StringBuilder();
            body.AppendLine(weatherResponse.Headline.Text);
            var loTemp = dailyForecast.Temperature.Minimum;
            var hiTemp = dailyForecast.Temperature.Maximum;
            if (loTemp.Unit == "F")
            {
                body.AppendLine($"Low: {loTemp.Value + loTemp.Unit}/{loTemp.Value.ToCelsius()}C");
                body.AppendLine($"Hi: {hiTemp.Value + loTemp.Unit}/{hiTemp.Value.ToCelsius()}C");
            }
            else
            {
                body.AppendLine($"Low: {loTemp.Value + loTemp.Unit}");
                body.AppendLine($"Hi: {hiTemp.Value + hiTemp.Unit}");
            }
            return body.ToString();
        }

        internal virtual async Task<string> ObtainLocationKey(IAccuWeatherSerivce accuWeatherService, string cityWithState, CancellationToken cancellationToken = default)
        {
            var cityResults = await accuWeatherService.SearchCity(cityWithState, cancellationToken);
            var closestMatch = cityResults.FirstOrDefault();
            return closestMatch.Key;
        }
    }
}

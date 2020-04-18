using System;
using System.Collections.Generic;
using System.Text;
using WeatherEmailer.Logic.Api.AccuWeatherService;
using WeatherEmailer.Configuration;
using WeatherEmailer.Logging;
using WeatherEmailer.Logic.Api.Interfaces;

namespace WeatherEmailer.Logic.Api
{
    public interface IServiceProxyFactory
    {
        IWeatherService GetWeatherService();
        IEmailService GetEmailService();
    }

    public class ServiceProxyFactory: IServiceProxyFactory
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        public ServiceProxyFactory(ILogger logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }
        public IWeatherService GetWeatherService()
        {
            if (_appSettings.AccuWeatherSettings.UseActualService)
                return new AccuWeatherSerivce(_logger, _appSettings);
            _logger.Log("Using fake AccuWeatherService");
            return new FakeWeatherService();
        }

        public IEmailService GetEmailService() => new GmailService(_appSettings, _logger);
    }
}

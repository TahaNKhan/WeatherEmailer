using System;
using System.Collections.Generic;
using System.Text;
using WeatherTextMessager.Configuration;
using WeatherTextMessager.Logging;

namespace WeatherTextMessager.Logic.Api
{
    public interface IServiceProxyFactory
    {
        IAccuWeatherSerivce GetAccuWeatherSerivce();
        IGmailSerivce GetGmailSerivce();
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
        public IAccuWeatherSerivce GetAccuWeatherSerivce()
        {
            if (_appSettings.AccuWeatherSettings.UseActualService)
                return new AccuWeatherSerivce(_logger, _appSettings);
            _logger.Log("Using fake AccuWeatherService");
            return new FakeAccuWeatherService();
        }

        public IGmailSerivce GetGmailSerivce() => new GmailSerivce(_appSettings, _logger);
    }
}

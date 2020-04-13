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
        public IAccuWeatherSerivce GetAccuWeatherSerivce() => new AccuWeatherSerivce(_logger, _appSettings);

        public IGmailSerivce GetGmailSerivce() => new GmailSerivce(_appSettings, _logger);
    }
}

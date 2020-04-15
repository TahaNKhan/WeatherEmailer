﻿using Microsoft.Extensions.DependencyInjection;
using System;
using WeatherEmailer.Logging;
using WeatherEmailer.Logic;
using WeatherEmailer.Logic.Api;
using Microsoft.Extensions.Configuration;
using System.IO;
using WeatherEmailer.Configuration;

namespace WeatherEmailer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile($"appsettings.json", optional: false)
                .Build();
            var appSettings = new AppSettings();
            config.GetSection("AppSettings").Bind(appSettings);
            var serviceProvider = new ServiceCollection()
               .AddSingleton<IServiceProxyFactory, ServiceProxyFactory>()
               .AddSingleton<ILogger, BasicLogger>()
               .AddSingleton(appSettings)
               .AddTransient<ISendWeatherInformationTask, SendWeatherInformationTask>()
               .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();
            logger.Log("Starting application...");

            try
            {
                //do the actual work here
                var task = serviceProvider.GetService<ISendWeatherInformationTask>();
                await task.SendWeatherInformation();
                logger.Log("All done!");
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString());
            }
            finally
            {
                logger.Publish();
            }
        }
    }
}

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherEmailer.Logic.Api.AccuWeatherService;
using WeatherEmailer.Logic.Api.AccuWeatherService.Models;
using WeatherEmailer.Configuration;
using WeatherEmailer.Logic.Api;
using WeatherEmailer.Logic.Helpers;
using WeatherEmailer.Persistence.Interfaces;
using System.Collections.Generic;
using WeatherEmailer.Persistence.Models;
using WeatherEmailer.Contracts;
using WeatherEmailer.Logging;
using System;

namespace WeatherEmailer.Logic
{
	public interface ISendWeatherInformationTask
	{
		Task SendWeatherInformation(CancellationToken cancellationToken = default);
	}
	public class SendWeatherInformationTask : ISendWeatherInformationTask
	{
		private readonly IServiceProxyFactory _serviceProxyFactory;
		private readonly AppSettings _appSettings;
		private readonly IDataContextFactory _dataContextFactory;
		private readonly ILogger _logger;
		public SendWeatherInformationTask(IServiceProxyFactory serviceProxyFactory, AppSettings appSettings, IDataContextFactory dataContextFactory, ILogger logger)
		{
			_serviceProxyFactory = serviceProxyFactory;
			_appSettings = appSettings;
			_dataContextFactory = dataContextFactory;
			_logger = logger;
		}

		public async Task SendWeatherInformation(CancellationToken cancellationToken = default)
		{
			var weatherService = _serviceProxyFactory.GetWeatherService();

			var users = await GetUserLocationsAsync();
			if (users == null || !users.Any())
			{
				_logger.Log("No user configurations found, exiting");
				return;
			}
			var weatherInfoCache = new Dictionary<string, WeatherInformation>();

			foreach(var user in users)
			{
				var cacheKey = user.City + user.State;
				if (!weatherInfoCache.TryGetValue(cacheKey, out WeatherInformation weatherInfo))
				{
					weatherInfo = await weatherService.GetWeatherInformationAsync(user.City, user.State, cancellationToken);
					weatherInfoCache.TryAdd(cacheKey, weatherInfo);
				}
					
				var subject = $"Weather Update - {user.City} {user.State}";
				var body = BuildEmailBody(weatherInfo, user);
				var emailService = _serviceProxyFactory.GetEmailService();
				await emailService.SendEmailAsync(user.Emails, subject, body.ToString());
			}
		}

		internal virtual async Task<IEnumerable<UserLocation>> GetUserLocationsAsync()
		{
			using var dataContext = _dataContextFactory.Construct();
			var dataProvider = dataContext.GetUserLocationDataProvider();
			var userLocations = await dataProvider.GetUserLocations();
			return userLocations;
		}

		internal virtual string BuildEmailBody(Contracts.WeatherInformation weatherInfo, UserLocation userLocation)
		{
			var body = new StringBuilder();
			body.AppendLine($"Hi {userLocation.UserName}!");
			body.AppendLine($"Here is your weather forecast for the day:{Environment.NewLine}");
			body.AppendLine(weatherInfo.Headline);
			var loTemp = weatherInfo.Temperature.Low;
			var hiTemp = weatherInfo.Temperature.High;
			if (loTemp.Unit == Contracts.TemperatureValue.TemperatureUnit.Fahrenheit)
			{
				body.AppendLine($"Low: {loTemp}/{loTemp.Value.ToCelsius()}C");
				body.AppendLine($"Hi: {hiTemp}/{hiTemp.Value.ToCelsius()}C");
			}
			else
			{
				body.AppendLine($"Low: {loTemp}");
				body.AppendLine($"Hi: {hiTemp}");
			}
			return body.ToString().Trim();
		}
	}
}

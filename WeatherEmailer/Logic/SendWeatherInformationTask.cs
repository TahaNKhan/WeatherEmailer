using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherEmailer.Logic.Api.AccuWeatherService;
using WeatherEmailer.Logic.Api.AccuWeatherService.Models;
using WeatherEmailer.Configuration;
using WeatherEmailer.Logic.Api;
using WeatherEmailer.Logic.Helpers;

namespace WeatherEmailer.Logic
{
    public interface ISendWeatherInformationTask
    {
        Task SendWeatherInformation(CancellationToken cancellationToken = default);
    }
    public class SendWeatherInformationTask: ISendWeatherInformationTask
    {
        private readonly IServiceProxyFactory _serviceProxyFactory;
        private readonly AppSettings _appSettings;
        public SendWeatherInformationTask(IServiceProxyFactory serviceProxyFactory, AppSettings appSettings)
        {
            _serviceProxyFactory = serviceProxyFactory;
            _appSettings = appSettings;
        }

        public async Task SendWeatherInformation(CancellationToken cancellationToken = default)
        {
            var weatherService = _serviceProxyFactory.GetWeatherService();
            var weatherInfo = await weatherService.GetWeatherInformationAsync(_appSettings.CurrentCity, _appSettings.CurrentState, cancellationToken);
            var subject = $"Weather Update - {_appSettings.CurrentCity} {_appSettings.CurrentState}";
            var body = BuildEmailBody(weatherInfo);
            var emailService = _serviceProxyFactory.GetEmailService();
            await emailService.SendEmailAsync(_appSettings.Emails, subject, body.ToString());
        }

        internal virtual string BuildEmailBody(Contracts.WeatherInformation weatherInfo)
        {
            var body = new StringBuilder();
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
            return body.ToString();
        }
    }
}

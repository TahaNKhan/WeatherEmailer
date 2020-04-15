using System.Threading;
using System.Threading.Tasks;
using WeatherEmailer.Logic.Api.Interfaces;
using WeatherEmailer.Contracts;

namespace WeatherEmailer.Logic.Api.AccuWeatherService
{
    public class FakeWeatherService : IWeatherService
    {
        public Task<WeatherInformation> GetWeatherInformationAsync(string city, string state, CancellationToken cancellationToken = default)
        {
            var result = new WeatherInformation
            {
                Headline = "Fake Weather Headline",
                Temperature = new Contracts.Temperature
                {
                    High = new TemperatureValue
                    {
                        Unit = TemperatureValue.TemperatureUnit.Fahrenheit,
                        Value = 55
                    },
                    Low = new TemperatureValue
                    { 
                        Unit = TemperatureValue.TemperatureUnit.Fahrenheit,
                        Value = 40
                    }
                }
            };
            return Task.FromResult(result);
        }
    }
}

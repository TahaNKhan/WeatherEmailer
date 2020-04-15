using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherEmailer.Logic.Api.Interfaces
{
    public interface IWeatherService
    {
        Task<Contracts.WeatherInformation> GetWeatherInformationAsync(string city, string state, CancellationToken cancellationToken = default);
    }
}

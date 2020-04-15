using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherEmailer.Logic.Api.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(IEnumerable<string> to, string subject, string body, CancellationToken cancellationToken = default);
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherTextMessager.Configuration;
using WeatherTextMessager.Logging;

namespace WeatherTextMessager.Logic.Api
{
    public interface IGmailSerivce
    {
        Task SendEmailAsync(IEnumerable<string> to, string subject, string body, CancellationToken cancellationToken = default);
    }
    public class GmailSerivce : IGmailSerivce, IDisposable
    {
        private readonly Logging.ILogger _logger;
        private readonly SmtpClient _smtpClient;
        private readonly AppSettings _appSettings;

        public GmailSerivce(AppSettings appSettings, ILogger logger)
        {
            _logger = logger;
            _appSettings = appSettings;
            _logger.Log("Connecting to smtp.gmail.com:587");
            _smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(appSettings.Email, appSettings.EmailPassword),
                EnableSsl = true
            };
            _logger.Log("Connected to gmail!");
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

        public async Task SendEmailAsync(IEnumerable<string> to, string subject, string body, CancellationToken cancellationToken = default)
        {
            try
            {
                var email = new MailMessage
                {
                    Body = body,
                    Subject = subject,
                    From = new MailAddress(_appSettings.Email, "Automation Helper")
                };
                _logger.Log($"Sending email to:");
                foreach (var toString in to)
                {
                    _logger.Log(toString);
                    email.Bcc.Add(new MailAddress(toString));
                }
                if (_appSettings.SendEmails)
                {
                    await _smtpClient.SendMailAsync(email);
                    _logger.Log("Email sent!");
                }
                else
                {
                    _logger.Log("Email not sent since it was specified in the configuration");
                }
            }
            catch (Exception)
            {
                _logger.Log("Failed to send email");
                throw;
            }
        }
    }
}

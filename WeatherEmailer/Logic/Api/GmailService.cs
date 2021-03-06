﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherEmailer.Configuration;
using WeatherEmailer.Logging;
using WeatherEmailer.Logic.Api.Interfaces;

namespace WeatherEmailer.Logic.Api
{
	public class GmailService : IEmailService
	{
		private readonly Logging.ILogger _logger;
		private readonly AppSettings _appSettings;

		public GmailService(AppSettings appSettings, ILogger logger)
		{
			_logger = logger;
			_appSettings = appSettings;
		}

		public async Task SendEmailAsync(IEnumerable<string> recepients, string subject, string body, CancellationToken cancellationToken = default)
		{
			using var smtpClient = BuildSmtpClient();
			_logger.Log($"Sending email to:");
			foreach (var to in recepients)
			{
				try
				{
					_logger.Log(to);
					var mailMessage = GenerateMailMessage(to, subject, body);
					if (_appSettings.SendEmails)
						await smtpClient.SendMailAsync(mailMessage);
					else
						_logger.Log($"Did not send email to {to} due to config settings");
				}
				catch (Exception ex)
				{
					_logger.Log($"Failed to send email to: {to}");
					_logger.Log(ex.ToString());
					continue;
				}
			}
			_logger.Log("Emails processed!");
		}

		internal virtual MailMessage GenerateMailMessage(string to, string subject, string body)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(_appSettings.Email, "Automation Helper"),
				Body = body,
				Subject = subject
			};
			mailMessage.To.Add(new MailAddress(to));
			return mailMessage;
		}

		internal virtual SmtpClient BuildSmtpClient()
		{
			_logger.Log("Connecting to smtp.gmail.com:587");
			var smtpClient = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential(_appSettings.Email, _appSettings.EmailPassword),
				EnableSsl = true
			};
			_logger.Log("Connected to gmail!");
			return smtpClient;
		}
	}
}

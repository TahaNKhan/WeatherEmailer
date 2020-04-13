using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeatherTextMessager.Configuration;

namespace WeatherTextMessager.Logging
{
    public interface ILogger
    {
        void Log(string str);
    }
    public class BasicLogger : ILogger
    {
        private readonly string _logFile;
        public BasicLogger(AppSettings appSettings)
        {
            _logFile = appSettings.LoggingLocation;
        }

        public void Log(string str)
        {
            str = str.Replace(Environment.NewLine, " ");
            str = str.Replace("\n", " ");
            str = str.Replace("\r", " ");

            File.AppendAllText(_logFile, $"{DateTimeOffset.Now:MMM dd yyyy HH:mm:ss zzz} - {str}{Environment.NewLine}");
        }
    }
}

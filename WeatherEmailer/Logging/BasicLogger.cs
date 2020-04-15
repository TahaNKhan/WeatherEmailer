using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeatherEmailer.Configuration;

namespace WeatherEmailer.Logging
{
    public interface ILogger
    {
        void Log(string str);
        void Publish();
    }
    public class BasicLogger : ILogger
    {
        private readonly string _logFile;
        private readonly StringBuilder _logs;
        public BasicLogger(AppSettings appSettings)
        {
            _logFile = appSettings.LoggingLocation;
            _logs = new StringBuilder();
        }

        public void Log(string str)
        {
            str = str.Replace(Environment.NewLine, " ");
            str = str.Replace("\n", " ");
            str = str.Replace("\r", " ");
            _logs.Append($"{DateTimeOffset.Now:MMM dd yyyy HH:mm:ss zzz} - {str}{Environment.NewLine}");
        }

        public void Publish()
        {
            File.AppendAllText(_logFile, _logs.ToString());
        }
    }
}

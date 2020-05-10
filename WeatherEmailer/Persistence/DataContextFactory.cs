using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WeatherEmailer.Configuration;
using WeatherEmailer.Persistence.Interfaces;

namespace WeatherEmailer.Persistence
{
	public class DataContextFactory : IDataContextFactory
	{
		private readonly AppSettings _appSettings;
		public DataContextFactory(AppSettings appSettings)
		{
			_appSettings = appSettings;
		}
		public IDataContext Construct()
		{
			var assemblyLocation = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
			
			return new FileDataContext(assemblyLocation);
		}
	}
}

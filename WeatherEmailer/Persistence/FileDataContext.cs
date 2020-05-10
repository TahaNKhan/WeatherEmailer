using System;
using System.Collections.Generic;
using System.IO;
using WeatherEmailer.Persistence.Interfaces;

namespace WeatherEmailer.Persistence
{
	public class FileDataContext : IDataContext
	{
		private readonly FileStream _fileStream;
		public FileDataContext(string path)
		{
			var filePath = path + "\\DataFile.json";
			_fileStream = File.Open(filePath, FileMode.OpenOrCreate);
		}

		public IUserLocationDataProvider GetUserLocationDataProvider()
		{
			var dataProvider =  new UserLocationDataProvider(_fileStream);
			return dataProvider;
		}

		public void Dispose() 
		{
			_fileStream.Dispose();
		}
	}
}
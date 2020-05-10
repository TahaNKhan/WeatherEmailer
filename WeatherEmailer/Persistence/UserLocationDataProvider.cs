using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WeatherEmailer.Persistence.Interfaces;
using WeatherEmailer.Persistence.Models;

namespace WeatherEmailer.Persistence
{
	public class UserLocationDataProvider : IUserLocationDataProvider
	{
		private readonly FileStream _fileStream;
		public UserLocationDataProvider(FileStream fileStream)
		{
			_fileStream = fileStream;
		}

		public async Task<IEnumerable<UserLocation>> GetUserLocations()
		{
			var reader = new StreamReader(_fileStream);
			var serialized = await reader.ReadToEndAsync();
			var result = JsonConvert.DeserializeObject<FileDataContextModel>(serialized);
			return result?.UserLocations;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherEmailer.Persistence.Models
{
	public class FileDataContextModel
	{
		public IEnumerable<UserLocation> UserLocations { get; set; }
	}
}

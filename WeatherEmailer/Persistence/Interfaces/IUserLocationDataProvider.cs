using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherEmailer.Persistence.Interfaces
{
	public interface IUserLocationDataProvider
	{
		Task<IEnumerable<Models.UserLocation>> GetUserLocations();
	}
}
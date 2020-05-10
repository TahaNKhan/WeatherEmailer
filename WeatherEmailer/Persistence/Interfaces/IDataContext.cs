using System;

namespace WeatherEmailer.Persistence.Interfaces
{
	public interface IDataContext : IDisposable
	{
		IUserLocationDataProvider GetUserLocationDataProvider();
	}
}
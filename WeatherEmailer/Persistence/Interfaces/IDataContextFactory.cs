namespace WeatherEmailer.Persistence.Interfaces
{
	public interface IDataContextFactory
	{
		IDataContext Construct();
	}
}

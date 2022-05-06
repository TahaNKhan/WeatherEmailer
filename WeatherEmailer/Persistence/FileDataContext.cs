using System.IO;
using WeatherEmailer.Persistence.Interfaces;

namespace WeatherEmailer.Persistence
{
    public class FileDataContext : IDataContext
    {
        private readonly FileStream _fileStream;
        public FileDataContext(string path)
        {
            var filePath = getDataFileName(path);
            _fileStream = File.Open(filePath, FileMode.OpenOrCreate);
        }

        public IUserLocationDataProvider GetUserLocationDataProvider()
        {
            var dataProvider = new UserLocationDataProvider(_fileStream);
            return dataProvider;
        }

        public void Dispose()
        {
            _fileStream.Dispose();
        }

		private string getDataFileName(string path) {
			const string releaseFileName = "DataFile.release.json";
			const string regularFileName = "DataFile.json";
			var fullReleaseFilePath = $"{path}\\{releaseFileName}";
			var fullRegularFilePath = $"{path}\\{regularFileName}";
			
			if (File.Exists(fullReleaseFilePath)) 
				return fullReleaseFilePath;
			
			return fullRegularFilePath;
		}
    }
}
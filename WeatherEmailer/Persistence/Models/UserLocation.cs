using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherEmailer.Persistence.Models
{
	public class UserLocation
	{
		public string UserName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public IEnumerable<string> Emails { get; set; }
	}
}

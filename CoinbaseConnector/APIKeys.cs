using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CoinbaseConnector
{
	class APIKeys
	{
		public static string API_KEY = ConfigurationManager.AppSettings["API_KEY"];
		public static string API_SECRET = ConfigurationManager.AppSettings["API_SECRET"];
	}
}

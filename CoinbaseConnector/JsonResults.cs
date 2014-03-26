using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseConnector
{
	public class CreateOAuthApp_Result
	{
		public Application[] applications { get; set; }
		public string total_count { get; set; }
		public string num_pages { get; set; }
		public string current_page { get; set; }
	}

	public class Application
	{
		public string id { get; set; }
		public string created_at { get; set; }
		public string name { get; set; }
		public string num_users { get; set; }
	}
}

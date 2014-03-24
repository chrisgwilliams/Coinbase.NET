using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using CoinbaseConnector;
using Newtonsoft.Json;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var cbc = new CoinbaseConnector.Connector();

			Console.WriteLine("Account Changes: ");
			Console.WriteLine(cbc.GetAccountChanges());
			Console.WriteLine("");

			Console.WriteLine("Account Balance: ");
			Console.WriteLine(cbc.GetAccountBalance());
			Console.WriteLine("");
			
			Console.WriteLine("Generate Receive Address: ");
			Console.WriteLine(cbc.GenerateReceiveAddress());
			Console.WriteLine("");

			Console.WriteLine("Current Receive Address: ");
			Console.WriteLine(cbc.GetCurrentReceiveAddress());
			Console.WriteLine("");
			
			Console.WriteLine("Generate Receive Address with Callback and Label: ");
			Console.WriteLine(cbc.GenerateReceiveAddress("http://www.example.com", "Sample"));
			Console.WriteLine("");

			Console.WriteLine("Current (New) Receive Address: ");
			Console.WriteLine(cbc.GetCurrentReceiveAddress());
			Console.WriteLine("");
			
			Console.WriteLine("Address List: ");
			Console.WriteLine(cbc.GetAddressList());
			Console.WriteLine("");

			Console.WriteLine("Create OAuth Application: ");
			Console.WriteLine(cbc.CreateOAuthApplication("SampleApp414", "http://www.example.com"));
			Console.WriteLine("");
			
			Console.WriteLine("OAuth Applications List: ");
			var Response = cbc.GetOAuthApplicationsList();
			Console.WriteLine(Response);
			Console.WriteLine("");

			CreateOAuthApp_Result result = JsonConvert.DeserializeObject<CreateOAuthApp_Result>(Response);
			foreach (Application application in result.applications)
			{
				Console.WriteLine("OAuth Application Info By ID: " + application.id);
				Console.WriteLine(cbc.GetOauthApplicationByID(application.id));
			}
			Console.WriteLine("");
			
			Console.WriteLine("Application Account Access Info: ");
			Console.WriteLine(cbc.GetApplicationAccountAccessInfo());
			Console.WriteLine("");



			Console.ReadLine();
		}
	}
}

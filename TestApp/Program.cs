using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoinbaseConnector;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var cbc = new CoinbaseConnector.Connector();

			Console.WriteLine("Balance: ");
			Console.WriteLine(cbc.GetAccountBalance());
			Console.WriteLine("");
			Console.WriteLine("Receiving Address: ");
			Console.WriteLine(cbc.GetCurrentReceiveAddress());
			Console.WriteLine("");
			Console.WriteLine("Generate Receive Address: ");
			Console.WriteLine(cbc.GenerateReceiveAddress());
			Console.WriteLine("");
			Console.WriteLine("Generate Receive Address with Callback and Label: ");
			Console.WriteLine(cbc.GenerateReceiveAddress("http://www.example.com", "Sample"));
			Console.WriteLine("");
			Console.WriteLine("Account Changes: ");
			Console.WriteLine(cbc.GetAccountChanges());
			Console.WriteLine("");
			Console.WriteLine("Addresse List: ");
			Console.WriteLine(cbc.GetAddressList());
			Console.WriteLine("");
			Console.WriteLine("OAuth Applications List: ");
			Console.WriteLine(cbc.GetOAuthApplicationsList());
			Console.WriteLine("");
			Console.WriteLine("Application Account Access Info: ");
			Console.WriteLine(cbc.GetApplicationAccountAccessInfo());
			Console.WriteLine("");

			Console.ReadLine();
		}
	}
}

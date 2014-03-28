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

	public class CreateButton_Result
	{
		public string success { get; set; }
		public Button button { get; set; }
	}

	public class Button
	{
		public string code { get; set; }
		public string type { get; set; }
		public string style { get; set; }
		public string text { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string custom { get; set; }
		public string callback_url { get; set; }
		public Price price { get; set; }
	}

	public class Price
	{
		public string cents { get; set; }
		public string currency_iso { get; set; }
	}

	public class CreateOrder_Result
	{
		public string success { get; set; }
		public Order order {get; set;}
	}

	public class Order
	{
		public string id { get; set; }
		public string created_at { get; set; }
		public string status { get; set; }
		public TotalMoney total_btc { get; set; }
		public TotalMoney totalNative { get; set; }
		public string custom { get; set; }
		public string receive_address {  get; set; }
		public ButtonShortResponse button { get; set; }
		public string transaction { get; set; }
	}

	public class TotalMoney
	{
		public string cents { get; set; }
		public string currency_iso { get; set; }
	}

	public class ButtonShortResponse
	{
		public string type { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string id { get; set; }
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseConnector
{
	
	public class AccountSettings_Result
	{
		public User[] users { get; set; }
	}

	public class User
	{
		public string id { get; set; }
		public string name { get; set; }
		public string email { get; set; }
		public string time_zone { get; set; }
		public string native_currency { get; set; }
		public Amount balance { get; set; }
		public string buy_level { get; set; }
		public string sell_level { get; set; }
		public Amount buy_limit { get; set; }
		public Amount sell_limit { get; set; }
	}

	public class CreateInvoice_Result
	{
		public string success { get; set; }
		public Transaction transaction { get; set; }
	}
	
	public class Transaction
	{
		public string id { get; set; }
		public string created_at { get; set; }
		public string hsh { get; set; }
		public string notes { get; set; }
		public Amount amount { get; set; }
		public string request { get; set; }
		public string status { get; set; }
		public Party sender { get; set; }
		public Party recipient { get; set; }
	}

	public class Party
	{
		public string id { get; set; }
		public string name { get; set; }
		public string email { get; set; }
	}

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

	public class RecurringPayments_Result
	{
		public RecurringPayment[] recurring_payments { get; set; }
		public string total_count { get; set; }
		public string num_pages { get; set; }
		public string current_page { get; set; }
	}

	public class RecurringPayment
	{
		public string id { get; set; }
		public string type { get; set; }
		public string status { get; set; }
		public string created_at { get; set; }
		public string to { get; set; }
		public string from { get; set; }
		public string start_type { get; set; }
		public string times { get; set; }
		public string times_run { get; set; }
		public string repeat { get; set; }
		public string last_run { get; set; }
		public string next_run { get; set; }
		public string notes { get; set; }
		public string description { get; set; }
		public Amount amount { get; set; }
	}

	public class Amount
	{
		public string amount { get; set; }
		public string currency { get; set; }
	}
}

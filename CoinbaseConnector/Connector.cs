using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace CoinbaseConnector
{
    public class Connector
    {
		private string API_KEY = APIKeys.API_KEY;
		private string API_SECRET = APIKeys.API_SECRET;

		private string URL_BASE = "https://coinbase.com/api/v1/";
		private const string GET = "GET";
		private const string POST = "POST";
		
		// Account Changes
		public string GetAccountChanges(String page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "account_changes?page=" + page, GET);
		}

		// Account
		public string GetAccountBalance()
		{
			return JsonRequest(URL_BASE + "account/balance", GET);
		}
		public string GetCurrentReceiveAddress()
		{
			return JsonRequest(URL_BASE + "account/receive_address", GET);
		}
		public string GenerateReceiveAddress()
		{
			return JsonRequest(URL_BASE + "account/generate_receive_address", POST);
		}
		public string GenerateReceiveAddress(String callbackURL, String label)
		{
			return JsonRequest(URL_BASE + "account/generate_receive_address?address[callback_url]=" + callbackURL+"&address[label]=" + label, POST); 
		}

		// Addresses
		public string GetAddressList(String page = "1", String limit = "25", String query = "")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "addresses?page=" + page + "&limit=" + limit + "&query=" + query, GET);
		}

		// OAuth Applications
		public string GetOAuthApplicationsList(String page = "1")
		{
			return JsonRequest(URL_BASE + "oauth/applications?page=" + page, GET);
		}
		public string GetOauthApplicationByID(String ID)
		{
			return JsonRequest(URL_BASE + "oauth/applications/" + ID, GET);
		}
		public string CreateOAuthApplication()
		{
			return JsonRequest(URL_BASE + "oauth/applications/", POST);
		}

		// Authorization
		public string GetAccountAuthorizationInfo()
		{
			return JsonRequest(URL_BASE + "authorization", GET);
		}

		// Buttons
		public string CreatePaymentButton()
		{
			string jsonParams=""; // TODO
			return JsonRequest(URL_BASE + "buttons", POST, jsonParams);
		}
		public string CreateOrderForButton()
		{
			string jsonParams=""; // TODO
			return JsonRequest(URL_BASE + "buttons/:code/create_order", POST, jsonParams);
		}

		// Buys
		public string PurchaseBitcoin()
		{
			string jsonParams=""; // TODO
			return JsonRequest(URL_BASE + "buys", POST, jsonParams);
		}

		// Contacts
		public string GetEmailContacts()
		{
			return JsonRequest(URL_BASE + "contacts", GET);
		}

		// Currencies
		public string GetSupportedCurrencies()
		{
			return JsonRequest(URL_BASE + "currencies", GET);
		}
		public string GetBTCExchangeRate()
		{
			return JsonRequest(URL_BASE + "exchange_rates", GET);
		}

		// Orders
		public string GetReceivedMerchantOrders(string page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "orders?page=" + page, GET);
		}
		public string CreateNewOrder()
		{
			string jsonParams = ""; // TODO
			return JsonRequest(URL_BASE + "orders", POST, jsonParams);
		}
		public string GetMerchantOrderByID(string ID = "")
		{
			return JsonRequest(URL_BASE + "orders/" + ID, GET);
		}

		// Payment Methods
		public string GetAssociatedPaymentMethods()
		{
			return JsonRequest(URL_BASE + "payment_methods", GET);
		}

		// Prices
		public string GetTotalBuyPriceForBitcoin(float qty = 1, String currency = "USD")
		{
			return JsonRequest(URL_BASE + "prices/buy?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetTotalSellPriceForBitcoin(float qty = 1, String currency = "USD")
		{
			return JsonRequest(URL_BASE + "prices/sell?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetSpotPriceForBitcoin(String currency = "USD")
		{
			// Currency must be an ISO 4217 Currency Code. Default is USD
			return JsonRequest(URL_BASE + "prices/spot_rate?currency=" + currency, GET);
		}
		public string GetHistoricalSpotPriceForBitcoin(String page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "prices/historical?page=" + page, GET);
		}

		// Recurring Payments
		public string GetRecurringPayments(String ID = "")
		{
			// ID field is optional. Default is no parameter. 
			return JsonRequest(URL_BASE + "recurring_payments/" + ID, GET);
		}

		// Sells
		public string SellBitcoin(float qty, String payment_method_id = "")
		{
			// Quantity of Bitcoin to sell is required.
			// Payment Method ID is optional. Will use default account ID. Must have verified bank account to work.
			return JsonRequest(URL_BASE + "sells?qty=" + qty + "&payment_method_id=" + payment_method_id, POST);	
		}



#region Supporting Classes for API Parameters

		public class Order
		{
			public Button button {get; set;}
		}

		public class Button
		{
			public string name { get; set; }
			public string price_string { get; set; }
			public string price_currency_iso { get; set; }
			public string type { get; set; }
			public string repeat { get; set; }
			public string style { get; set; }
			public string text { get; set; }
			public string description { get; set; }
			public string custom { get; set; }
			public bool custom_secure { get; set; }
			public string callback_url { get; set; }
			public string success_url { get; set; }
			public string cancel_url { get; set; }
			public string info_url { get; set; }
			public bool auto_redirect { get; set; }
			public bool variable_price { get; set; }
			public bool choose_price { get; set; }
			public bool include_address { get; set; }
			public bool include_email { get; set; }
			public string price1 { get; set; }
			public string price2 { get; set; }
			public string price3 { get; set; }
			public string price4 { get; set; }
			public string price5 { get; set; }
		}

#endregion


		private string JsonRequest(string url, string method, String postdata = "")
		{
			string returnData = String.Empty;

			// strip out any escape characters in the optional parameters
			postdata = postdata.Replace(@"\", "");

			var webRequest = HttpWebRequest.Create(url) as HttpWebRequest;
			if (webRequest != null)
			{
				webRequest.Accept = "*/*";
				webRequest.UserAgent = ".NET";
				webRequest.Method = method;
				webRequest.ContentType = "application/json";
				webRequest.Host = "coinbase.com";

				string nonce = Convert.ToInt64(DateTime.Now.Ticks).ToString();
				string message = nonce + url + postdata;
				string signature = HashEncode(HashHMAC(StringEncode(API_SECRET), StringEncode(message)));

				var whc = new WebHeaderCollection();
				whc.Add("ACCESS_KEY: " + API_KEY);
				whc.Add("ACCESS_SIGNATURE: " + signature);
				whc.Add("ACCESS_NONCE: " + nonce);
				webRequest.Headers = whc;

				if (postdata != "")
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[] byte1 = encoding.GetBytes(postdata);

					// Set the content length of the string being posted.
					webRequest.ContentLength = byte1.Length;
					Stream newStream = webRequest.GetRequestStream();

					newStream.Write(byte1, 0, byte1.Length);
				}

				using (WebResponse response = webRequest.GetResponse())
				{
					using (Stream stream = response.GetResponseStream())
					{
						StreamReader reader = new StreamReader(stream);
						returnData = reader.ReadToEnd();
					}
				}
			}

			return returnData;
		}

		private static byte[] StringEncode(string text)
		{
			var encoding = new ASCIIEncoding();
			return encoding.GetBytes(text);
		}

		private static string HashEncode(byte[] hash)
		{
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}

		private static byte[] HashHMAC(byte[] key, byte[] message)
		{
			var hash = new HMACSHA256(key);
			return hash.ComputeHash(message);
		}
    }
}
